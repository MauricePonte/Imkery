using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [Route("api/hives")]
    [ApiController]
    public class HiveController : BaseController<HiveRepository, Hive>
    {
        public HiveController(HiveRepository repository) : base(repository)
        {
        }
    }
}
