using Imkery.Data.Storage.Core;
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
            var repositoryType = typeof(EFRepository);
            foreach (var type in typeof(StorageStartupExtensions).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && repositoryType.IsAssignableFrom(t)))
            {
                serviceCollection.AddScoped(type);
            }
        }

    }
}
