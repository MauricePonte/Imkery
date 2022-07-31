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
        public BaseCRUDClient(string apiArea, HttpClient httpClient, IApiConfiguration settings) : base(httpClient, settings)
        {
            _area = apiArea;
        }

        public async Task<T?> GetItemAsync(Guid id)
        {
            var response = await (await GetHttpClient().ConfigureAwait(false)).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }

        public virtual async Task<ICollection<T>?> GetCollectionAsync(FilterPagingOptions filterPagingOptions)
        {
            return await GetItemsAsync(filterPagingOptions);
        }

        public async Task<ICollection<T>?> GetItemsAsync(FilterPagingOptions filterPagingOptions, bool withAutherization = true)
        {
            var response = await (await GetHttpClient().ConfigureAwait(false)).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "?filterpaging=", filterPagingOptions.ToQueryString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ICollection<T>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return null;
        }

        public async Task<int> GetCountAsync(FilterPagingOptions filterPagingOptions)
        {
            var response = await (await GetHttpClient().ConfigureAwait(false)).GetAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/count", "?filterpaging=", filterPagingOptions.ToQueryString())).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return 0;
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await (await GetHttpClient().ConfigureAwait(false)).DeleteAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString())).ConfigureAwait(false);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ThrowResponseException(response);
            }
        }

        public async Task<T?> AddAsync(T item)
        {
            var response = await (await GetHttpClient().ConfigureAwait(false)).PostAsync(string.Concat(_settings.GetAPIEndPoint(), _area), new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }
        public async Task<T?> EditAsync(Guid id, T item)
        {
            var response = await (await GetHttpClient().ConfigureAwait(false)).PutAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/", id.ToString()), new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return default;
        }
    }
}
