using Imkery.API.Client.Core;
using Imkery.Entities;

namespace Imkery.API.Client
{
    public class HiveClient : BaseCRUDClient<Hive>
    {
        public HiveClient(HttpClient httpClient, IApiConfiguration settings)
            : base("hives", httpClient, settings)
        {
        }
    }
}