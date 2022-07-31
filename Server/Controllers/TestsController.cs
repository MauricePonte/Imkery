using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Imkery.Server.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestsController : BaseController<TestRepository, Test>
    {
        public TestsController(TestRepository repository) : base(repository)
        {
        }
    }
}
