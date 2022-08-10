using FluentValidation;
using Imkery.Entities;

namespace Imkery.Entities
{
    public class TagDefinition : IEntity<TagDefinition>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string GetDescription() => Name;

        public AbstractValidator<TagDefinition> GetValidator()
        {
            var validator = new InlineValidator<TagDefinition>();
            validator.RuleFor(b => b.Name).NotEmpty();
            return validator;
        }
    }
}
