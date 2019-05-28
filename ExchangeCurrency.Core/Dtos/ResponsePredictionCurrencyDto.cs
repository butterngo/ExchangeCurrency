namespace ExchangeCurrency.Core.Dtos
{
    using System;

    public class ResponsePredictionCurrencyDto
    {
        public ResponsePredictionCurrencyDto() { }

        public ResponsePredictionCurrencyDto(DateTime predictionDate,
            double squared,
            double intercept,
            double slope)
        {
            PredictionDate = predictionDate;

            Squared = squared;

            Intercept = intercept;

            Slope = slope;
        }

        public string PredictionDateValue => PredictionDate.ToStr();

        public DateTime PredictionDate { get; set; }

        public double Squared { get; set; }

        public double Intercept { get; set; }

        public double Slope { get; set; }

        public string FromCurrency { get; set; }

        public string toCurrency { get; set; }

        public double PredictedValue
        {
            get
            {
                return Math.Round((Slope * 13) + Intercept, 3);
            }
        }
    }
}
