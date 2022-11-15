using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imkery.Data.Storage
{
    public class LocationsRepository : EFRepository<Location>
    {
        public LocationsRepository(ImkeryDbContext dbContext, IImkeryUserProvider userProvider) : base(dbContext, userProvider)
        {
        }

        public override DbSet<Location> DbSet => DbContext.Locations;

        public override async Task<IQueryable<Location>> ApplyFilteringAsync(IQueryable<Location> query, Dictionary<string, string> filterValues)
        {
            var predicate = PredicateBuilder.True<Location>();

            if (filterValues.ContainsKey("Name") && !string.IsNullOrWhiteSpace(filterValues["Name"]))
            {
                var keywords = filterValues["identifier"].Split(",");
                foreach (string keyword in keywords)
                    predicate = predicate.Or(p => p.Name.Contains(keyword));
            }

            return query.Where(predicate);
        }


        public override void ConfigureModel(EntityTypeBuilder<Location> modelBuilder)
        {
        }
    }
}
