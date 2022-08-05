using Imkery.Data.Storage;
using Imkery.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : BaseController<LocationRepository, Location>
    {
        public LocationController(LocationRepository repository) : base(repository)
        {
        }
    }
}