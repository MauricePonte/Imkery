using Imkery.API.Client.Core;
using Imkery.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Imkery.API.Client
{
    public class HivesClient : BaseCRUDClient<Hive>
    {
        public HivesClient(IHttpClientFactory httpClient, IApiConfiguration settings)
            : base("hives", (options) => { }, httpClient, settings)
        {
        }


        public async Task<Hive?> ApplyActionToHiveAsync(Hive hive, ActionDefinition action)
        {
            var response = await GetHttpClient(true).PostAsync(string.Concat(_settings.GetAPIEndPoint(), _area, "/DoAction/", hive.Id.ToString(), "/", action.Id.ToString()), null).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Hive>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            ThrowResponseException(response);
            return null;
        }


    }
}