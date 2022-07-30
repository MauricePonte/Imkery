﻿using Imkery.API.Client.Core;
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
        private Dictionary<Type, BaseClient> _registry { get; set; } = new Dictionary<Type, BaseClient>();
        public ApiClientRegistry(LocationClient locationClient)
        {

            _registry.Add(typeof(Location), locationClient);
        }

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
