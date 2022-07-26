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
        public ApiClientRegistry(BeesClient imkeryClient)
        {
            _registry.Add(typeof(Bee), imkeryClient);
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
