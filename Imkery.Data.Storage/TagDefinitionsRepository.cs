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
    public class TagDefinitionsRepository : EFRepository<TagDefinition>
    {
        public TagDefinitionsRepository(ImkeryDbContext dbContext, IImkeryUserProvider userProvider) : base(dbContext, userProvider)
        {
        }

        public override DbSet<TagDefinition> DbSet => DbContext.TagDefinitions;

        public override IQueryable<TagDefinition> ApplyFiltering(IQueryable<TagDefinition> query, Dictionary<string, string> filterValues)
        {
            if (filterValues.ContainsKey("searchText") && !string.IsNullOrWhiteSpace(filterValues["searchText"]))
            {
                query = query.Where(b => EF.Functions.Like(b.Name, $"%{filterValues["searchText"]}%"));
            }
            return query;
        }

        public override void ConfigureModel(EntityTypeBuilder<TagDefinition> modelBuilder)
        {
        }
    }
}
