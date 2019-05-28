namespace ExchangeCurrency.Core.Providers
{
    using ExchangeCurrency.Core.Dtos;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class OXRProvider
    {
        private const string APP_ID = "0d06bf0a72d64560bd0a873aa355071a";

        private const string DEFAULT_FORMAT_DATE_TIME = "yyyy-MM-dd";

        private readonly HttpClient _client;

        public OXRProvider()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<OXRDto> GetHistoricalAsync(DateTime date)
        {
            return await GetHistoricalAsync(date.ToStr(DEFAULT_FORMAT_DATE_TIME));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<OXRDto> GetHistoricalAsync(string date)
        {
            string endPoint = $"https://openexchangerates.org/api/historical/{string.Format(date)}.json?app_id={APP_ID}";

            var response = await _client.GetAsync(endPoint);

            if (response.StatusCode != HttpStatusCode.OK) throw new ExchangeCurrencyException("Bad request");

            var json = await response.Content.ReadAsStringAsync();

            var dto = JsonConvert.DeserializeObject<OXRDto>(json);

            dto.Date = date.ToDateTime(DEFAULT_FORMAT_DATE_TIME);

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<OXRDto> GetLatestAsync()
        {
            string endPoint = $"https://openexchangerates.org/api/latest.json?app_id={APP_ID}";

            var response = await _client.GetAsync(endPoint);

            if (response.StatusCode != HttpStatusCode.OK) throw new ExchangeCurrencyException("Bad request");

            var json = await response.Content.ReadAsStringAsync();

            var dto = JsonConvert.DeserializeObject<OXRDto>(json);

            dto.Date = DateTime.Now;

            return dto;
        }

    }
}
