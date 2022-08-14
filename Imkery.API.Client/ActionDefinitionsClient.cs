using Imkery.API.Client.Core;
using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public class ActionDefinitionsClient : BaseCRUDClient<ActionDefinition>
    {
        public ActionDefinitionsClient(IHttpClientFactory httpClient, IApiConfiguration settings)
            : base("actiondefinitions", (options) => { }, httpClient, settings)
        {
        }
    }
}
