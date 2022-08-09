using Imkery.API.Client.Core;
using Imkery.Entities;

namespace Imkery.API.Client
{
    public class HiveClient : BaseCRUDClient<Hive>
    {
        public HiveClient(IHttpClientFactory httpClient, IApiConfiguration settings)
            : base("hives", (options) => { }, httpClient, settings)
        {
        }
    }
}