using Imkery.API.Client.Core;
using Imkery.Entities;

namespace Imkery.API.Client
{
    public class LocationsClient : BaseCRUDClient<Location>
    {
        public LocationsClient(IHttpClientFactory httpClient, IApiConfiguration settings)
            : base("locations", (options) => { options.AuthorizationOptions.ForGetCollection = false; }, httpClient, settings)
        {
        }
    }
}