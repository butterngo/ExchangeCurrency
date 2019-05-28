namespace ExchangeCurrency.UnitTest
{
    using ExchangeCurrency.Core.Dtos;
    using ExchangeCurrency.Core.Providers;
    using ExchangeCurrency.Core.Services;
    using Xunit;

    public class PredictCurrencyUnitTest
    {
        private readonly ExchangeService _service;

        public PredictCurrencyUnitTest()
        {
            _service = new ExchangeService();

        }

        [Fact]
        public void PredictTest()
        {
            string fromCurrency = "USD", toCurrency = "TRY", dateTime = "15/01/2017";

            double expectationRate = 3.263;

            var result = _service.PredictCurrencyAsync(fromCurrency, toCurrency, dateTime).Result;

            Assert.IsType<ResponsePredictionCurrencyDto>(result);

            Assert.NotNull(result);

            Assert.Equal(fromCurrency, result.FromCurrency);

            Assert.Equal(toCurrency, result.toCurrency);

            Assert.Equal(dateTime, result.PredictionDateValue);

            Assert.Equal(expectationRate, result.PredictedValue);
        }
    }
}
