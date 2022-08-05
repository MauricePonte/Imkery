using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Location : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string GetDescription() => Name;
    }
}
