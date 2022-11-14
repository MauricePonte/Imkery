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

        public bool IsContinues { get; set; } = true;
        public string? Duration { get; set; }

        public string GetDescription() => Id.ToString();

        public AbstractValidator<TagLink> GetValidator()
        {
            var validator = new InlineValidator<TagLink>();
            validator.RuleFor(b => b.Duration).Matches("[0-9][0-9]:[0-9][0-9]").When(b=> !b.IsContinues);
            return validator;
        }
    }
}
