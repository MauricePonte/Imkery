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
    }
}