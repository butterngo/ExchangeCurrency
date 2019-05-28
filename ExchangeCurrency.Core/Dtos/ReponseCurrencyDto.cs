namespace ExchangeCurrency.Core.Dtos
{
    public class ReponseCurrencyDto
    {
        public ReponseCurrencyDto() { }

        public ReponseCurrencyDto(string fromCurrency,
            string toCurrency,
            double rate,
            string date)
        {
            FromCurrency = fromCurrency;

            ToCurrency = toCurrency;

            Rate = rate;

            Date = date;
        }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public double Rate { get; set; }

        public string Date { get; set; }

        public override string ToString()
        {
            return $" The predicted currency exchange from {FromCurrency.ToUpper()} "
                + $" to {ToCurrency.ToUpper()} for {Date} is {Rate}. ";
        }
    }
}
