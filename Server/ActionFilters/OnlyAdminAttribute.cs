using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server
{
    public class OnlyAdminAttribute : TypeFilterAttribute
    {
        public OnlyAdminAttribute() : base(typeof(OwnerOrAdminTypeFilter))
        {
            Arguments = new object[] {
               typeof(OnlyAdminAttribute),
               "",
               true
            };
        }
    }
}
