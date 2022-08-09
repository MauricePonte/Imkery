using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/hives")]
    public class HiveController : BaseController<HiveRepository, Hive>
    {
        public HiveController(HiveRepository repository) : base(repository)
        {
        }
    }
}
