using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public interface IImkeryUser
    {
        string UserName { get; set; }
        string Id { get; }
        Guid GuidId { get => Guid.Parse(Id); }
    }
}
