using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/location")]
    public class LocationController : BaseController<LocationRepository, Location>
    {
        public LocationController(LocationRepository repository) : base(repository)
        {
        }
    }
}