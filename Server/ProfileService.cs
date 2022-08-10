using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Imkery.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Imkery.Server
{
    public class ProfileService : ProfileService<ApplicationUser>
    {
        public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
        {
        }

        protected override async Task<ClaimsPrincipal> GetUserClaimsAsync(ApplicationUser user)
        {
            var principal = await base.GetUserClaimsAsync(user);
            var identity = new ClaimsIdentity((ClaimsIdentity)principal.Identity);
            if (user.IsAdministrator)
            {
                identity.AddClaim(new Claim("role", "Admin"));

            }

            return new ClaimsPrincipal(identity);
        }
    }
}
