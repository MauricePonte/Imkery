using FluentValidation;
using Imkery.Entities;

namespace Imkery.Entities
{
    public class TagDefinition : IEntity<TagDefinition>, IEquatable<TagDefinition?>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public Guid OwnerId { get; set; }

        public string GetDescription() => Name;


        public override bool Equals(object otherObject)
        {
            var other = otherObject as TagDefinition;
            return other?.Id == Id;
        }

        public AbstractValidator<TagDefinition> GetValidator()
        {
            var validator = new InlineValidator<TagDefinition>();
            validator.RuleFor(b => b.Name).NotEmpty();
            return validator;
        }

        public bool Equals(TagDefinition? other)
        {
            return other is not null &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
