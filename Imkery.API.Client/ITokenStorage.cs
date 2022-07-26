using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public interface ITokenStorage
    {
        public Task StoreToken(string token);
        public Task<string> GetToken();   
    }
}
