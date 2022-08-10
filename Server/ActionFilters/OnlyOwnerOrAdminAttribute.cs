using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server
{
    public class OnlyOwnerOrAdminAttribute : TypeFilterAttribute
    {
        public OnlyOwnerOrAdminAttribute(Type repositoryType, string idOrObjectParameter, bool onlyAdmin = false) : base(typeof(OwnerOrAdminTypeFilter))
        {
            Arguments = new object[] {
               repositoryType,
               idOrObjectParameter ?? "",
               onlyAdmin
            };
        }
    }
}
