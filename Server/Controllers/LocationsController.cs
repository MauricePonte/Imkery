using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/locations")]
    public class LocationsController : BaseController<LocationsRepository, Location>
    {
        public LocationsController(LocationsRepository repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public override Task<ActionResult<ICollection<Location>>> GetCollectionAsync([FromQuery(Name = "filterpaging")] string filterPagingOptionsJson)
        {
            return base.GetCollectionAsync(filterPagingOptionsJson);
        }
    }
}