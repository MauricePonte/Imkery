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
    public class BeeRepository : EFRepository<Bee>
    {
        public BeeRepository(ImkeryDbContext dbContext) : base(dbContext)
        {
        }

        public override DbSet<Bee> DbSet => DbContext.Bees;

        public override IQueryable<Bee> ApplyFiltering(IQueryable<Bee> query, Dictionary<string, string> filterValues)
        {
            if (filterValues.ContainsKey("searchText") && !string.IsNullOrWhiteSpace(filterValues["searchText"]))
            {
                query = query.Where(b => EF.Functions.Like(b.Name, $"%{filterValues["searchText"]}%"));
            }
            return query;
        }

        public override void ConfigureModel(EntityTypeBuilder<Bee> modelBuilder)
        {
        }
    }
}
