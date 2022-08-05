using Imkery.API.Client.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public static class ClientStartupExtensions
    {
        public static void AddImkeryAPIClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped((services)=>
            {
                return new ApiClientRegistry(services);
            });
            var clientType = typeof(BaseClient);
            foreach (var type in typeof(ClientStartupExtensions).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && clientType.IsAssignableFrom(t)))
            {
                serviceCollection.AddScoped(type);

            }
        }
    }
}
