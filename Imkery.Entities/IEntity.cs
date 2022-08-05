using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imkery.Entities
{
    public interface IEntity<T> : IEntity where T : class, new()
    {
        public Guid Id { get; set; }
        AbstractValidator<T> GetValidator();
        string GetDescription();
    }

    public interface IEntity
    {

    }
}
