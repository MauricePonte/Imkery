using Imkery.API.Client.Core;
using Imkery.Entities;

namespace Imkery.API.Client
{
    public class LocationClient : BaseCRUDClient<Location>
    {

        public LocationClient(HttpClient httpClient, IApiConfiguration settings)
            : base("bees", httpClient, settings)
        {
        }

    }
}