using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class TagLink : IEntity<TagLink>
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public Guid TagDefinitionId { get; set; }
        public TagDefinition TagDefinition { get; set; }


        public string GetDescription() => Id.ToString();

        public AbstractValidator<TagLink> GetValidator()
        {
            return new InlineValidator<TagLink>();
        }
    }
}
