using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client.Core
{
    public abstract class BaseClient
    {
        HttpClient _httpClient;
        protected IApiConfiguration _settings;
        public BaseClient(HttpClient httpClient, IApiConfiguration settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        //Warning will be removed bt making it async, but waiting if we want to read cookies or other things that need async
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<HttpClient> GetHttpClient()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return _httpClient;

        }

        protected void ThrowResponseException(HttpResponseMessage response)
        {
            ExceptionOnApiCall?.Invoke(response);
        }

        public static Action<HttpResponseMessage>? ExceptionOnApiCall { get; set; }

    }
}
