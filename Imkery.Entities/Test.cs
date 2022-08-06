using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Test : IEntity
    {

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string GetDescription() => Name;
    }
}
