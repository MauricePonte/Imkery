using Imkery.API.Client.Core;
using Imkery.Entities;

namespace Imkery.API.Client
{
    public class LocationClient : BaseCRUDClient<Location>
    {
        public LocationClient(IHttpClientFactory httpClient, IApiConfiguration settings)
            : base("location", (options) => { }, httpClient, settings)
        {
        }
    }
}