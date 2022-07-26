using Imkery.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imkery.Data.Storage
{
    public static class StorageStartupExtensions
    {
        public static void AddImkeryRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<BeeRepository>();
        }

    }
}
