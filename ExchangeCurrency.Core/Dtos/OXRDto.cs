namespace ExchangeCurrency.Core.Dtos
{
    using Newtonsoft.Json.Linq;
    using System;

    public class OXRDto
    {
        public string Disclaimer { get; set; }

        public string License { get; set; }

        public string Timestamp { get; set; }

        public string Base { get; set; }

        public JObject Rates { get; set; }

        public DateTime Date { get; set; }

        public double GetRate(string currency)
        {
            var property = Rates.Property(currency.ToUpper());

            if (property == null) throw new Exception($"Not found {currency}");
            
            return (double)property.Value;
        }

        public double CalculateRate(string fromCurrency, string toCurrency)
        {
            var rateOfFromCurrency = GetRate(fromCurrency);

            var rateOfToCurrency = GetRate(toCurrency);

            return Math.Round(rateOfToCurrency / rateOfFromCurrency, 6);
        }
    }
}
