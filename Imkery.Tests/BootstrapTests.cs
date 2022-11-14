using Imkery.API.Client;
using Imkery.Data.Storage.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Tests
{
    [SetUpFixture]
    public class BootstrapTests
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public BootstrapTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ImkeryDbContext>(options =>
        options.UseInMemoryDatabase("ImkeryTestDb"));
            services.AddImkeryRepositories();
            services.AddImkeryAPIClients();
            services.AddScoped<IImkeryUserProvider, MockUserProvider>();
            ServiceProvider = services.BuildServiceProvider();
        }

    }
}
