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
    public class ActionDefinitionsRepository : EFRepository<ActionDefinition>
    {
        public ActionDefinitionsRepository(ImkeryDbContext dbContext, IImkeryUserProvider userProvider) : base(dbContext, userProvider)
        {
            SeperatePerUser = true;
        }

        public override DbSet<ActionDefinition> DbSet => DbContext.ActionDefinitions;

        public override IQueryable<ActionDefinition> ApplyFiltering(IQueryable<ActionDefinition> query, Dictionary<string, string> filterValues)
        {
            if (filterValues.ContainsKey("searchText") && !string.IsNullOrWhiteSpace(filterValues["searchText"]))
            {
                query = query.Where(b => EF.Functions.Like(b.Name, $"%{filterValues["searchText"]}%"));
            }
            return query;
        }

        public override void ConfigureModel(EntityTypeBuilder<ActionDefinition> modelBuilder)
        {
        }

        public override async Task<ActionDefinition> GetItemByIdAsync(Guid id)
        {
            return await GetItemById(id, new string[] { "TagLinks.TagDefinition"});
        }
        public override async Task<ActionDefinition> AddAsync(ActionDefinition entity)
        {
            var currentUser = await UserProvider.GetCurrentUserAsync();
            if (entity.TagLinks != null && currentUser != null)
            {
                foreach (TagLink link in entity.TagLinks)
                {
                    link.OwnerId = currentUser.GuidId;
                }
            }
            return await base.AddAsync(entity);
        }

        public override async Task<ActionDefinition> UpdateAsync(Guid id, ActionDefinition entity)
        {
            var currentUser = await UserProvider.GetCurrentUserAsync();
            if (entity.TagLinks != null && currentUser != null)
            {
                foreach (TagLink link in entity.TagLinks)
                {
                    link.OwnerId = currentUser.GuidId;
                    link.TagDefinition = null;
                }
            }
            return await base.UpdateAsync(id, entity);
        }
    }
}
