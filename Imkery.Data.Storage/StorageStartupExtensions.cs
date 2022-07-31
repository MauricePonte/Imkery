using FluentValidation;
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

        private static void AddRepository<TRepository, TEntity>(IServiceCollection serviceCollection) where TRepository : EFRepository<TEntity>
                                                                                                      where TEntity : class, IEntity<TEntity>, new()
        {
            serviceCollection.AddScoped<TRepository>();
            serviceCollection.AddScoped<IValidator<TEntity>>((serviceProvider) => serviceProvider.GetService<TRepository>().GetValidator());
        }
    }
}
