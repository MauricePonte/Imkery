using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Route("api/tests")]
    [Authorize]
    public class TestsController : BaseController<TestRepository, Test>
    {
        public TestsController(TestRepository repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public override Task<ActionResult<ICollection<Test>>> GetCollectionAsync([FromQuery(Name = "filterpaging")] string filterPagingOptionsJson)
        {
            return base.GetCollectionAsync(filterPagingOptionsJson);
        }

        [AllowAnonymous]
        public override Task<ActionResult<int>> GetCountAsync([FromQuery(Name = "filterpaging")] string filterPagingOptionsJson)
        {
            return base.GetCountAsync(filterPagingOptionsJson);
        }
    }
}
