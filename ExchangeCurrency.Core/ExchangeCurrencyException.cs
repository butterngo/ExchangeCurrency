namespace ExchangeCurrency.Core
{
    using System;

    public class ExchangeCurrencyException: Exception
    {
        public ExchangeCurrencyException() { }

        public ExchangeCurrencyException(string message) : base(message) { }
    }
}
