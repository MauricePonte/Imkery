using Imkery.API.Client.Core;
using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public class ApiClientRegistry
    {

        public ApiClientRegistry(IServiceProvider services)
        {
            FillClientRegistry(services);
        }

        public void FillClientRegistry(IServiceProvider services)
        {
            var clientType = typeof(BaseClient);
            foreach (var type in typeof(ClientStartupExtensions).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && clientType.IsAssignableFrom(t)))
            {
                var entityType = FindEntityType(type);

                if (entityType != null)
                {
                    BaseClient? client = services.GetService(type) as BaseClient;
                    if (client != null)
                    {
                        _registry.Add(entityType, client);
                    }
                }

            }
        }

        private Type? FindEntityType(Type type)
        {
            var entityType = type.GenericTypeArguments.FirstOrDefault(b => typeof(IEntity).IsAssignableFrom(b));
            if (entityType == null)
            {
                if (type.BaseType == typeof(BaseClient) || type.BaseType == null)
                {
                    return null;
                }
                return FindEntityType(type.BaseType);
            }
            return entityType;
        }

        private Dictionary<Type, BaseClient> _registry { get; set; } = new Dictionary<Type, BaseClient>();

        public BaseClient GetApiClientForType(Type objectType)
        {
            return _registry[objectType];
        }
        public T GetApiClientForTypeAs<T>(Type objectType) where T : BaseClient
        {
            return (T)_registry[objectType];
        }
    }
}
