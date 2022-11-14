using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imkery.Entities
{
    public class Hive : IEntity<Hive>
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public virtual ICollection<Tag> Tags { get; set; }

        public string GetDescription()
        {
            return Identifier;
        }

        public AbstractValidator<Hive> GetValidator()
        {
            var validator = new InlineValidator<Hive>
            {
                builder => builder.RuleFor(b => b.Identifier).NotEmpty()
            };
            return validator;
        }
    }
}