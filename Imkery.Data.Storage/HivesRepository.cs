using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imkery.Data.Storage
{
    public class HivesRepository : EFRepository<Hive>
    {
        public HivesRepository(ImkeryDbContext dbContext, IImkeryUserProvider userProvider) : base(dbContext, userProvider)
        {
        }

        public override DbSet<Hive> DbSet => DbContext.Hives;

        public override IQueryable<Hive> ApplyFiltering(IQueryable<Hive> query, Dictionary<string, string> filterValues)
        {
            var predicate = PredicateBuilder.True<Hive>();

            if (filterValues.ContainsKey("identifier") && !string.IsNullOrWhiteSpace(filterValues["identifier"]))
            {
                
                var keywords = filterValues["identifier"].Split(",");
                foreach (string keyword in keywords)
                    predicate = predicate.Or(p => p.Identifier.Contains(keyword));
            }

            return query.Where(predicate);
        }

        public override void ConfigureModel(EntityTypeBuilder<Hive> modelBuilder)
        {
        }
    }
}