using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client.Core
{
    public abstract class BaseCRUDClient<T> : BaseClient
    {
        private string _area;
        private BaseCRUDClientOptions Options { get; set; }
        public BaseCRUDClient(string apiArea, Action<BaseCRUDClientOptions> options, IHttpClientFactory httpClientFactory, IApiConfiguration settings) : base(httpClientFactory, settings)
        {
            _area = apiArea;
            Options = new BaseCRUDClientOptions();
            options.Invoke(Options);
        }

        public async virtual Task<T> GetItemAsync(Guid id)
        {
            var response = await GetHttpClient(Options.AuthorizationOptions.ForGetItem).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }

        public virtual async Task<ICollection<T>> GetCollectionAsync(FilterPagingOptions filterPagingOptions)
        {
            return await GetItemsAsync(filterPagingOptions);
        }

        public async virtual Task<ICollection<T>> GetItemsAsync(FilterPagingOptions filterPagingOptions)
        {
            var response = await GetHttpClient(Options.AuthorizationOptions.ForGetCollection).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "?filterpaging=", filterPagingOptions.ToQueryString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ICollection<T>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return null;
        }

        public async virtual Task<int> GetCountAsync(FilterPagingOptions filterPagingOptions)
        {
            var response = await GetHttpClient(Options.AuthorizationOptions.ForGetCollection).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/count", "?filterpaging=", filterPagingOptions.ToQueryString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return 0;
        }

        public async virtual Task DeleteAsync(Guid id)
        {
            var response = await GetHttpClient(Options.AuthorizationOptions.ForDelete).DeleteAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString())).ConfigureAwait(false);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ThrowResponseException(response);
            }
        }

        public async virtual Task<T> AddAsync(T item)
        {
            var response = await GetHttpClient(Options.AuthorizationOptions.ForAdd).PostAsync(string.Concat(_settings.GetAPIEndPoint(), _area), new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }
        public async virtual Task<T> EditAsync(Guid id, T item)
        {
            var response = await GetHttpClient(Options.AuthorizationOptions.ForEdit).PutAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString()), new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }
    }

    public class BaseCRUDClientOptions
    {
        public AuthorizationOptions AuthorizationOptions { get; } = new AuthorizationOptions();
    }

    public class AuthorizationOptions
    {
        public bool ForEdit { get; set; } = true;
        public bool ForAdd { get; set; } = true;
        public bool ForDelete { get; set; } = true;
        public bool ForGetItem { get; set; } = true;
        public bool ForGetCollection { get; set; } = true;
    }
}
