using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Data.Storage
{
    public class TestRepository : EFRepository<Test>
    {
        public TestRepository(ImkeryDbContext dbContext, IImkeryUserProvider userProvider) : base(dbContext, userProvider)
        {
            SeperatePerUser = true;
        }

        public override DbSet<Test> DbSet => DbContext.Tests;

        public override IQueryable<Test> ApplyFiltering(IQueryable<Test> query, Dictionary<string, string> filterValues)
        {
            return query;
        }

        public override void ConfigureModel(EntityTypeBuilder<Test> modelBuilder)
        {
        }
     
    }
}
