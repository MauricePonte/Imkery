using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [Route("api/bees")]
    [ApiController]
    public class BeesController : BaseController<BeeRepository, Bee>
    {
        public BeesController(BeeRepository repository) : base(repository)
        {
        }
    }
}
