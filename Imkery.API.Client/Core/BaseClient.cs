using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client.Core
{
    public class BaseClient
    {
        HttpClient _httpClient;
        protected IApiConfiguration _settings;
        public BaseClient(HttpClient httpClient, IApiConfiguration settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            return _httpClient;

        }

        protected void ThrowResponseException(HttpResponseMessage response)
        {
            ExceptionOnApiCall?.Invoke(response);
        }

        public static Action<HttpResponseMessage> ExceptionOnApiCall { get; set; }

    }
}
