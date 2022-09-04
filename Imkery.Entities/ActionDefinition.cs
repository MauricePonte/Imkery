using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class ActionDefinition : IEntity<ActionDefinition>
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public virtual ICollection<TagLink>? TagLinks { get; set; }
        public string GetDescription() => Name;

        public AbstractValidator<ActionDefinition> GetValidator()
        {
            return new InlineValidator<ActionDefinition>();
        }
    }
}
