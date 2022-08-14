using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Route("api/actiondefinitions")]
    [Authorize]
    public class ActionDefinitionsController : BaseController<ActionDefinitionsRepository, ActionDefinition>
    {
        public ActionDefinitionsController(ActionDefinitionsRepository repository) : base(repository)
        {
        }
    }
}
