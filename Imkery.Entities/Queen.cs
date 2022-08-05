using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Queen : IEntity
    {
        public Guid Id { get; set; }

        public bool IsMarked { get; set; }

        public bool HasMated { get; set; }

        public string MarkingColor { get; set; } = string.Empty;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}
