using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class Test : IEntity<Test>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string GetDescription() => Name;

        public AbstractValidator<Test> GetValidator()
        {
            var validator = new InlineValidator<Test>();
            validator.Add(builder=> builder.RuleFor(b => b.Name).NotEmpty());
            validator.Add(builder => builder.RuleFor(b => b.Age).GreaterThan(18).WithMessage("Must be more than 18")
                                                                .LessThan(90).WithMessage("Must be less than 90"));
            return validator;
        }
    }
}
