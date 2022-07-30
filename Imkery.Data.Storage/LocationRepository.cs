using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imkery.Data.Storage
{
    internal class LocationRepository : EFRepository<Location>
    {
        public LocationRepository(ImkeryDbContext dbContext) : base(dbContext)
        {
        }

        public override DbSet<Location> DbSet => DbContext.Locations;

        public override IQueryable<Location> ApplyFiltering(IQueryable<Location> query, Dictionary<string, string> filterValues)
        {
            if (filterValues.ContainsKey("searchText") && !string.IsNullOrWhiteSpace(filterValues["searchText"]))
            {
                query = query.Where(b => EF.Functions.Like(b.Name, $"%{filterValues["searchText"]}%"));
            }
            return query;
        }

        public override void ConfigureModel(EntityTypeBuilder<Location> modelBuilder)
        {
        }
    }
}
