using FluentValidation;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Imkery.Data.Storage.Core
{
    public static class StorageStartupExtensions
    {
        public static void AddImkeryRepositories(this IServiceCollection serviceCollection)
        {
            var repositoryType = typeof(EFRepository);
            foreach (var type in typeof(StorageStartupExtensions).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && repositoryType.IsAssignableFrom(t)))
            {
                AddRepository(serviceCollection, type);
            }
        }

        private static Type? FindEntityType(Type type)
        {
            var entityType = type.GenericTypeArguments.FirstOrDefault(b => typeof(IEntity).IsAssignableFrom(b));
            if (entityType == null)
            {
                if (type.BaseType == typeof(EFRepository) || type.BaseType == null)
                {
                    return null;
                }
                return FindEntityType(type.BaseType);
            }
            return entityType;
        }
        private static void AddRepository(IServiceCollection serviceCollection, Type repositoryType)
        {
            serviceCollection.AddScoped(repositoryType);
            var entityType = FindEntityType(repositoryType);
            Type validatorType = typeof(IValidator<>);
            validatorType = validatorType.MakeGenericType(entityType);
            serviceCollection.AddScoped(validatorType, (serviceProvider) => (serviceProvider.GetService(repositoryType) as EFRepository).GetValidatorAbstract());
        }
    }
}
