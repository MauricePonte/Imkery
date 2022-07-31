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
        }

        private static void AddRepository<TRepository, TEntity>(IServiceCollection serviceCollection) where TRepository : EFRepository<TEntity>
                                                                                                      where TEntity : class, IEntity<TEntity>, new()
        {
            serviceCollection.AddScoped<TRepository>();
            serviceCollection.AddScoped<IValidator<TEntity>>((serviceProvider) => serviceProvider.GetService<TRepository>().GetValidator());
        }
    }
}
