using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public class BeesClient : BaseCRUDClient<Bee>
    {
        public BeesClient( HttpClient httpClient, ITokenStorage tokenProvider, IApiConfiguration settings) 
            : base("bees", httpClient, tokenProvider, settings)
        {
        }

    }
}
