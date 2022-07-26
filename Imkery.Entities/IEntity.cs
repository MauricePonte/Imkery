using System;
using System.Collections.Generic;
using System.Text;

namespace Imkery.Entities
{
    public interface IEntity
    {
        public Guid Id { get; set; }

        string GetDescription();
    }
}
