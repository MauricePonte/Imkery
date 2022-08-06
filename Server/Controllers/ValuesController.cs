using Imkery.Data.Storage.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly IImkeryUserProvider _userProvider;

        public ValuesController(IImkeryUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Test()
        {
            var user = await _userProvider.GetCurrentUserAsync();
            if (user != null)
            {
                return user.GuidId.ToString(); 
            }
            else
            {
                return "not logged in";
            }
        }
    }
}
