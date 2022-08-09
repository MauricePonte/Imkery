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
        IHttpClientFactory _httpClientFactory;
        protected IApiConfiguration _settings;
        public BaseClient(IHttpClientFactory httpClientFactory, IApiConfiguration settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings;
        }

        public HttpClient GetHttpClient(bool authenticated = true)
        {
            if (authenticated)
            {
                return _httpClientFactory.CreateClient(_settings.GetAuthenticatedHttpClientName());
            }
            return _httpClientFactory.CreateClient();

        }

        protected void ThrowResponseException(HttpResponseMessage response)
        {
            ExceptionOnApiCall?.Invoke(response);
        }

        public static Action<HttpResponseMessage> ExceptionOnApiCall { get; set; }

    }
}
