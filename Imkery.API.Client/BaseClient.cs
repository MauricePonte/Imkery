using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public class BaseClient
    {
        ITokenStorage _tokenProvider;
        HttpClient _httpClient;
        protected IApiConfiguration _settings;
        public BaseClient(HttpClient httpClient, ITokenStorage tokenProvider, IApiConfiguration settings)
        {
            _tokenProvider = tokenProvider;
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<HttpClient> GetHttpClient(bool useToken = true)
        {
            if (useToken)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetToken());
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;

            }
            return _httpClient;

        }

        protected void ThrowResponseException(HttpResponseMessage response)
        {
            ExceptionOnApiCall?.Invoke(response);
        }

        public static Action<HttpResponseMessage> ExceptionOnApiCall { get; set; }

    }
}
