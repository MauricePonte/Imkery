using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Tag : IEntity<Tag>
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public TagDefinition TagDefinition { get; set; }
        public Guid TagDefinitionId { get; set; }

        public DateTime AddedOn { get; set; }
        public DateTime ValidTill { get; set; }
        public bool AlwaysValid { get; set; }

        public string GetDescription()
        {
            return Id.ToString();
        }

        public AbstractValidator<Tag> GetValidator()
        {
            var validator = new InlineValidator<Tag>();
            validator.RuleFor(b => b.Id).NotEmpty();
            return validator;
        }
    }
}
