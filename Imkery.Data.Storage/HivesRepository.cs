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
        public override Task<Hive> GetItemById(Guid id, string[] includes)
        {
            return base.GetItemById(id, new string[] { "Tags.TagDefinition" });
        }

        public async Task<Hive> ApplyActionToHiveAsync(Hive hive, ActionDefinition action)
        {
            foreach (TagLink tagLink in action.TagLinks)
            {
                TimeSpan duration = new TimeSpan();
                if (!tagLink.IsContinues)
                {
                    string[] parts = tagLink.Duration.Split(":");
                    duration = new TimeSpan(int.Parse(parts[0]), int.Parse(parts[1]), 0);
                }

                hive.Tags.Add(new Tag()
                {
                    OwnerId = (await UserProvider.GetCurrentUserAsync()).GuidId,
                    Id = new Guid(),
                    TagDefinitionId = tagLink.TagDefinition.Id,
                    AddedOn = DateTime.UtcNow,
                    AlwaysValid = tagLink.IsContinues,
                    ValidTill = DateTime.UtcNow.Add(duration)
                });
            }
            return await UpdateAsync(hive.Id, hive);
        }
    }
}