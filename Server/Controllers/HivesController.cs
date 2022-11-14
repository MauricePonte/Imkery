using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Microsoft.AspNetCore.Mvc.Route("api/hives")]
    public class HivesController : BaseController<HivesRepository, Hive>
    {
        [Inject]
        public ActionDefinitionsRepository ActionDefinitionsRepository { get; set; }
        public HivesController(HivesRepository repository) : base(repository)
        {
        }

        [HttpPost("DoAction/{hiveId}/{actionId}")]
        public async Task<ActionResult<Hive>> ApplyActionToHiveAsync(Guid hiveId, Guid actionId)
        {
            if(actionId == Guid.Empty || actionId == Guid.Empty)
            {
                return BadRequest();
            }
            var hivesRepository = (_repository as HivesRepository);
            var hive = await hivesRepository.GetItemByIdAsync(hiveId);
            if(hive == null)
            {
                return BadRequest();
            }

            var action = await ActionDefinitionsRepository.GetItemByIdAsync(actionId);
            if (action == null)
            {
                return BadRequest();
            }
            return Ok(await hivesRepository.ApplyActionToHiveAsync(hive, action));

        }
    }
}
