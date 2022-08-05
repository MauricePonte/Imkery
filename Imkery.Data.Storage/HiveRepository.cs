using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imkery.Data.Storage
{
    public class HiveRepository : EFRepository<Hive>
    {
        public HiveRepository(ImkeryDbContext dbContext) : base(dbContext)
        {
        }

        public override DbSet<Hive> DbSet => DbContext.Hives;

        public override IQueryable<Hive> ApplyFiltering(IQueryable<Hive> query, Dictionary<string, string> filterValues)
        {
            if (filterValues.ContainsKey("searchText") && !string.IsNullOrWhiteSpace(filterValues["searchText"]))
            {
                query = query.Where(b => EF.Functions.Like(b.Identifier, $"%{filterValues["searchText"]}%"));
            }
            return query;
        }

        public override void ConfigureModel(EntityTypeBuilder<Hive> modelBuilder)
        {
        }
    }
}