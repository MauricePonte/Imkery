using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Hive : IEntity<Hive>
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public Guid LocationId { get; set; }
        public Location? Location { get; set; }

        [NotMapped]
        public ICollection<string> Tags { get; set; } = new List<string>();

        public string GetDescription()
        {
            return Identifier;
        }

        public AbstractValidator<Hive> GetValidator()
        {
            return null; 
        }
    }
}