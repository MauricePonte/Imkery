using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Imkery.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Imkery.Server
{
    public class ImkeryUserProvider : IImkeryUserProvider
    {
        public IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;
        public ImkeryUserProvider(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public async Task<IImkeryUser?> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if(user == null)
            {
                return null;
            }
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(user.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.NameIdentifier, StringComparison.InvariantCultureIgnoreCase))?.Value);
            return applicationUser;
        }
    }
}
