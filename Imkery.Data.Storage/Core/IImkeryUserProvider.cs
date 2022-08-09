using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Data.Storage.Core
{
    public interface IImkeryUserProvider
    {
        Task<IImkeryUser?> GetCurrentUserAsync();
    }
}
