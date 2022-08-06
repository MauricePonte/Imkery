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
        public BaseCRUDClient(string apiArea, IHttpClientFactory httpClientFactory, IApiConfiguration settings) : base(httpClientFactory, settings)
        {
            _area = apiArea;
        }

        public async virtual Task<T> GetItemAsync(Guid id, bool withAuthorization = true)
        {
            var response = await GetHttpClient(withAuthorization).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString())).ConfigureAwait(false);
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

        public async virtual Task<ICollection<T>> GetItemsAsync(FilterPagingOptions filterPagingOptions, bool withAuthorization = true)
        {
            var response = await GetHttpClient(withAuthorization).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "?filterpaging=", filterPagingOptions.ToQueryString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ICollection<T>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return null;
        }

        public async virtual Task<int> GetCountAsync(FilterPagingOptions filterPagingOptions, bool withAuthorization = true)
        {
            var response = await GetHttpClient().GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/count", "?filterpaging=", filterPagingOptions.ToQueryString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return 0;
        }

        public async virtual Task DeleteAsync(Guid id, bool withAuthorization = true)
        {
            var response = await GetHttpClient(withAuthorization).DeleteAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString())).ConfigureAwait(false);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ThrowResponseException(response);
            }
        }

        public async virtual Task<T> AddAsync(T item, bool withAuthorization = true)
        {
            var response = await GetHttpClient(withAuthorization).PostAsync(string.Concat(_settings.GetAPIEndPoint(), _area), new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }
        public async virtual Task<T> EditAsync(Guid id, T item, bool withAuthorization = true)
        {
            var response = await GetHttpClient(withAuthorization).PutAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString()), new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }
    }
}
