namespace ExchangeCurrency.Core
{
    using System;

    public static class DateTimeHelper
    {
        private const string DEFAULT_FORMAT_DATE_TIME = "dd/MM/yyyy";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, string format = DEFAULT_FORMAT_DATE_TIME)
        {
            return DateTime.ParseExact(value, format, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStr(this DateTime value, string format = DEFAULT_FORMAT_DATE_TIME)
        {
            return value.ToString(format);
        }

        /// <summary>
        /// If totalMonths greater 12 we should implement backgroud queue 
        /// refer https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static int CalculateMonths(string fromDate, string toDate)
        {
            return CalculateMonths(fromDate.ToDateTime(), toDate.ToDateTime());
        }

        /// <summary>
        /// If totalMonths greater 12 we should implement backgroud queue 
        /// refer https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static int CalculateMonths(DateTime fromDate, DateTime toDate)
        {
            var ts = toDate.Subtract(fromDate);

            var totalMonths = (int)(ts.TotalDays / 365.25 * 12);

            if (totalMonths < 0) throw new ExchangeCurrencyException("Invalid DateTime");

            if (totalMonths > 12) throw new ExchangeCurrencyException("Not support now");

            return totalMonths;
        }
    }
}
