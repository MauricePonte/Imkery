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
        public ApiClientRegistry(TestsClient testsClient)
        {
            _registry.Add(typeof(Test), testsClient);
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
