using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Hive : IEntity
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public Guid QueenId { get; set; }
        public Queen? Queen { get; set; }

        public Guid LocationId { get; set; }
        public Location? Location { get; set; }

        [NotMapped]
        public ICollection<string> Tags { get; set; } = new List<string>();

        public string GetDescription()
        {
            return Identifier;
        }
    }
}