using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Entities
{
    public class LocalImkeryUser
    {
        public string EmailAdress { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }

    }
}
