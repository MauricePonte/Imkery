using Imkery.API.Client.Core;
using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public class TestsClient : BaseCRUDClient<Test>
    {
        public TestsClient(IHttpClientFactory httpClient, IApiConfiguration settings) : base("tests", (options) =>
        {
            options.AuthorizationOptions.ForGetCollection = true;
        }, httpClient, settings)
        {
        }
    }
}
