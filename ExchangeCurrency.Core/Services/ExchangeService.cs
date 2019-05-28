namespace ExchangeCurrency.Core.Services
{
    using ExchangeCurrency.Core.Dtos;
    using ExchangeCurrency.Core.Providers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExchangeService
    {
        private readonly OXRProvider _provider;

        public ExchangeService():this(new OXRProvider()) { }

        public ExchangeService(OXRProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <returns></returns>
        public async Task<ReponseCurrencyDto> GetCurrentCurrency(string fromCurrency, string toCurrency)
        {
            OXRDto result = await _provider.GetLatestAsync();

            return new ReponseCurrencyDto(fromCurrency,
                toCurrency,
                result.CalculateRate(fromCurrency, toCurrency),
                DateTime.Now.ToStr());
        }


        public async Task<ResponsePredictionCurrencyDto> PredictCurrencyAsync(string fromCurrency,
            string toCurrency,
            string dateTime)
        {
            var currencies = await GetCurrencies(fromCurrency, toCurrency, dateTime);

            var xValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var yValues = currencies.Select(x => x.CalculateRate(fromCurrency, toCurrency)).ToList();

            Task ReCalculateValue(List<int> x, List<double> y)
            {
                do
                {
                    x = new List<int>();

                    for (int i = 0; i < yValues.Count; i++)
                    {
                        x.Add(i + 1);
                    };

                    var result = PredictCurrency(x, y, dateTime, ReCalculateValue);

                    y.Add(result.PredictedValue);

                } while (xValues.Count > yValues.Count);
                

                return Task.FromResult(0);
            }
            
            var response = PredictCurrency(xValues, yValues, dateTime, ReCalculateValue);

            response.FromCurrency = fromCurrency;

            response.toCurrency = toCurrency;

            return response;
        }

        private ResponsePredictionCurrencyDto PredictCurrency(List<int> xValues,
            List<double> yValues,
            string dateTime,
            Func<List<int>, List<double>, Task> callBack)
        {
            if (xValues.Count > yValues.Count) callBack.Invoke(xValues, yValues);

            double rSquared, intercept, slope;

            LinearRegression(xValues.ToArray(), yValues.ToArray(), out rSquared, out intercept, out slope);

            return new ResponsePredictionCurrencyDto(dateTime.ToDateTime(), rSquared, intercept, slope);
        }

        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="rSquared">The r^2 value of the line.</param>
        /// <param name="yIntercept">The y-intercept value of the line (i.e. y = ax + b, yIntercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        private void LinearRegression(
            int[] xVals,
            double[] yVals,
            out double rSquared,
            out double yIntercept,
            out double slope)
        {
            if (xVals.Length != yVals.Length)
            {
                throw new Exception("Input values should be with the same length.");
            }

            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            for (var i = 0; i < xVals.Length; i++)
            {
                var x = xVals[i];
                var y = yVals[i];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            var count = xVals.Length;
            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <param name="from">dd-MM-yyyy</param>
        /// <param name="to">dd-MM-yyyy</param>
        /// <returns></returns>
        private async Task<OXRDto[]> GetCurrencies(string fromCurrency,
            string toCurrency,
            string dateTime)
        {
            var toDate = dateTime.ToDateTime();

            var fromDate = toDate.AddMonths(-12);

            if (DateTime.Compare(toDate, DateTime.Now) >= 0)
            {
                fromDate = DateTime.Now.AddMonths(-DateTimeHelper.CalculateMonths(DateTime.Now, toDate));

                toDate = DateTime.Now;
            }
            
            var listTasks = new List<Task<OXRDto>>();

            for (var i = 0; i < DateTimeHelper.CalculateMonths(fromDate, toDate); i++)
            {
                var date = i == 0 ? fromDate : fromDate.AddMonths(+i);

                listTasks.Add(_provider.GetHistoricalAsync(date));
            }

            return await Task.WhenAll(listTasks);
        }
    }
}
