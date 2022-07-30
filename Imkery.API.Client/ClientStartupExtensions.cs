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
            serviceCollection.AddScoped<LocationClient>();
            serviceCollection.AddScoped<ApiClientRegistry>();
        }
    }
}
