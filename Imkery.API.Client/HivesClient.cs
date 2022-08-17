using Imkery.API.Client.Core;
using Imkery.Entities;

namespace Imkery.API.Client
{
    public class HivesClient : BaseCRUDClient<Hive>
    {
        public HivesClient(IHttpClientFactory httpClient, IApiConfiguration settings)
            : base("hives", (options) => { }, httpClient, settings)
        {
        }
    }
}