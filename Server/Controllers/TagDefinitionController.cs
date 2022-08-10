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
    public class TagDefinitionController : BaseController<TagDefinitionRepository, TagDefinition>
    {
        public TagDefinitionController(TagDefinitionRepository repository) : base(repository)
        {
        }

        [OnlyOwnerOrAdmin(typeof(TagDefinitionRepository), null, true)]
        public override Task<ActionResult<TagDefinition>> AddAsync([FromBody] TagDefinition entity)
        {
            return base.AddAsync(entity);
        }

        [OnlyOwnerOrAdmin(typeof(TagDefinitionRepository), "id", true)]
        public override Task<ActionResult<TagDefinition>> UpdateAsync(Guid id, [FromBody] TagDefinition entity)
        {
            return base.UpdateAsync(id, entity);
        }

        [OnlyOwnerOrAdmin(typeof(TagDefinitionRepository), "id", true)]
        public override Task<ActionResult> DeleteItemByIdAsync(Guid id)
        {
            return base.DeleteItemByIdAsync(id);
        }
    }
}
