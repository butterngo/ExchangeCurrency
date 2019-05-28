namespace ExchangeCurrency
{
    using ExchangeCurrency.Core.Services;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ExchangeService();

            bool isContinue = true;

            do
            {
                Console.Write("From currency: ");

                string fromCurrency = Console.ReadLine();

                Console.WriteLine("***************");

                Console.Write("To currency: ");

                string toCurrency = Console.ReadLine();

                Console.WriteLine("***************");

                Console.Write("Prediction date Format (dd/mm/yyyy): ");

                string dateTime = Console.ReadLine();

                var result = service.PredictCurrencyAsync(fromCurrency, toCurrency, dateTime).Result;

                Console.WriteLine("***************");

                Console.WriteLine($"The predicted currency exchange from {fromCurrency.ToUpper()} "
                    + $" to {toCurrency.ToUpper()} for {dateTime} is {result.PredictedValue}.");

                Console.WriteLine("***************");

                Console.Write("Do you want to continue? (y/n) ");

                isContinue = Console.ReadLine().ToUpper() == "Y" ? true : false;

            } while (isContinue);
           

            Console.ReadKey();
        }
    }
}
