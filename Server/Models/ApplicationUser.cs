using Imkery.Entities;
using Microsoft.AspNetCore.Identity;

namespace Imkery.Server.Models
{
    public class ApplicationUser : IdentityUser, IImkeryUser
    {
        public bool IsAdministrator { get; set; }
    }
}