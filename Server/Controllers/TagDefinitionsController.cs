using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Route("api/tagdefinitions")]
    [Authorize]
    public class TagDefinitionsController : BaseController<TagDefinitionsRepository, TagDefinition>
    {
        public TagDefinitionsController(TagDefinitionsRepository repository) : base(repository)
        {
        }

        [OnlyAdmin]
        public override Task<ActionResult<TagDefinition>> AddAsync([FromBody] TagDefinition entity)
        {
            return base.AddAsync(entity);
        }

        [OnlyAdmin]
        public override Task<ActionResult<TagDefinition>> UpdateAsync(Guid id, [FromBody] TagDefinition entity)
        {
            return base.UpdateAsync(id, entity);
        }

        [OnlyAdmin]
        public override Task<ActionResult> DeleteItemByIdAsync(Guid id)
        {
            return base.DeleteItemByIdAsync(id);
        }
    }
}
