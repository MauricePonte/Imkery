using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Location : IEntity<Location>
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string GetDescription() => Name;

        public AbstractValidator<Location> GetValidator()
        {
            var validator = new InlineValidator<Location>();
            validator.Add(builder => builder.RuleFor(b => b.Name).NotEmpty());
            validator.Add(builder => builder.RuleFor(b => b.Street).NotEmpty());
            validator.Add(builder => builder.RuleFor(b => b.City).NotEmpty());
            return validator;
        }
    }
}
