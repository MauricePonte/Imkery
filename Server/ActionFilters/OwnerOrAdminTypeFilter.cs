using IdentityModel;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Imkery.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Imkery.Server
{
    public class OwnerOrAdminTypeFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly bool _onlyAdmin;
        private readonly string _idOrObjectParameter;
        private readonly Type _repositoryType;

        public OwnerOrAdminTypeFilter(UserManager<ApplicationUser> userManager, Type repositoryType, string idOrObjectParameter, bool onlyAdmin)
        {
            _userManager = userManager;
            _onlyAdmin = onlyAdmin;
            _idOrObjectParameter = idOrObjectParameter;
            _repositoryType = repositoryType;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var loggedInUser = await GetLoggedInUser(context, _userManager);
            if (loggedInUser == null
                || ((_onlyAdmin && !loggedInUser.IsAdministrator) || (!loggedInUser.IsAdministrator && !(await MayLoggedInUserDoChange(loggedInUser, context)))))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else
            {
                await next();
            }
        }

        private async Task<bool> MayLoggedInUserDoChange(ApplicationUser loggedInUser, ActionExecutingContext context)
        {
            if (!string.IsNullOrWhiteSpace(_idOrObjectParameter))
            {
                Guid id = Guid.Empty;
                if(context.ActionArguments[_idOrObjectParameter] is Guid actionArgument)
                {
                    id = actionArgument;
                }
                else if(context.ActionArguments[_idOrObjectParameter] is IEntity actionArgumentEntity)
                {
                    id = actionArgumentEntity.Id;
                }
                else
                {
                    return false;
                }

                if (context.HttpContext.Request.Method == "DELETE")
                {
                    return await (context.HttpContext.RequestServices.GetService(_repositoryType) as EFRepository).CheckIfMemberMayDeleteObject(id, loggedInUser);

                }
                return await (context.HttpContext.RequestServices.GetService(_repositoryType) as EFRepository).CheckIfMemberMayChangeObject(id, loggedInUser);

            }
            else
            {
                return false;
            }
        }

        public static async Task<ApplicationUser> GetLoggedInUser(ActionContext context, UserManager<ApplicationUser> userManager)
        {
            var user = context?.HttpContext?.User;
            if (user == null)
            {
                return null;
            }
            ApplicationUser applicationUser = await userManager.FindByIdAsync(user.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.NameIdentifier, StringComparison.InvariantCultureIgnoreCase))?.Value);
            return applicationUser;
        }
    }
}
