
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using Imkery.Entities;

namespace Imkery.Data.Storage.Core
{
    public abstract class EFRepository
    {
        public EFRepository(ImkeryDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public ImkeryDbContext DbContext
        {
            get; set;
        }
    }
    public abstract class EFRepository<TItem> : EFRepository where TItem : class, IEntity, new()
    {

        public EFRepository(ImkeryDbContext dbContext) : base(dbContext)
        {
        }
        public abstract void ConfigureModel(EntityTypeBuilder<TItem> modelBuilder);


        public static void ChangeDbContext(ImkeryDbContext databaseContext)
        {

        }

        public abstract DbSet<TItem> DbSet { get; }
        public virtual async Task<TItem> AddAsync(TItem entity)
        {
            BuildChangeGraph(entity);
            //await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            UntrackItem(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(TItem entity)
        {
            BeforeItemDeleted(entity);
            //DbContext.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        public virtual void BeforeItemDeleted(TItem entity)
        {

        }
        public abstract IQueryable<TItem> ApplyFiltering(IQueryable<TItem> query, Dictionary<string, string> filterValues);

        public virtual async Task<ICollection<TItem>> GetCollectionAsync(int from, int count, string? sortField, bool desc, Dictionary<string, string> filterValues, string[]? includes)
        {
            var collection = ApplyFiltering(DbSet, filterValues);
            if (includes != null)
            {
                foreach (string include in includes)
                {
                    collection = collection.Include(include);
                }
            }

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                collection = collection.OrderByDynamic(sortField, !desc);
            }
            collection = collection.Skip(from).Take(count);
            return await collection.AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<TItem?> GetItemByIdAsync(Guid id)
        {
            return await GetItemById(id);
        }

        public virtual async Task<TItem?> GetItemById(Guid id, string[]? includes = null)
        {
            if (includes == null || includes.Length == 0)
            {
                return await DbSet.FindAsync(id);
            }
            else
            {
                var item = await DbSet.FindAsync(id);
                if (item == null)
                {
                    return null;
                }
                foreach (string include in includes)
                {
                    DbContext.Entry(item).Reference(include).Load();
                }
                return item;
            }

        }

        public virtual void CancelEdit(TItem entity)
        {
            UntrackItem(entity);
        }

        public virtual void UntrackItem(TItem entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
            var entries = DbContext.ChangeTracker.Entries().ToList();
            foreach (EntityEntry dbEntityEntry in entries)
            {
                dbEntityEntry.State = EntityState.Detached;
            }
        }

        public virtual async Task<TItem?> UpdateAsync(Guid id, TItem entity)
        {
            UntrackItem(entity);
            BuildChangeGraph(entity);
            //DbSet.Update(entity);
            foreach (var collection in DbContext.Entry(entity).Collections.ToList())
            {
                var loadedEntity = await DbSet.FindAsync(id);
                if (loadedEntity == null)
                {
                    continue;
                }
                var entry = DbContext?.Entry(loadedEntity);
                if (entry != null)
                {
                    entry.Reference(collection.Metadata.Name).Load();
                    entry.State = EntityState.Detached;
                }

                var dbCollection = (loadedEntity?.GetType()?.GetProperty(collection.Metadata.Name)?.GetValue(loadedEntity) as IEnumerable<object>)?.ToList();
                if(dbCollection == null)
                {
                    continue;
                }
                var currenentValues = collection.CurrentValue?.Cast<object>();
                foreach (var itemInDb in dbCollection)
                {
                    if (currenentValues?.Where(b => GetKey(b) == GetKey(itemInDb)).Count() == 0)
                    {
                        DbContext?.Remove(itemInDb);
                    }
                }
            }
            await DbContext.SaveChangesAsync();
            UntrackItem(entity);
            var item = await DbSet.FindAsync(id).ConfigureAwait(false);
            return item;
        }

        public virtual Guid GetKey<TEntity>(TEntity entity)
        {
            var keyName = DbContext?.Model?.FindEntityType(typeof(TItem))?.FindPrimaryKey()?.Properties
                .Select(x => x.Name).Single();
            if (keyName !=null && entity?.GetType()?.GetProperty(keyName)?.GetValue(entity, null) is Guid guidValue)
            {
                return guidValue;
            }
            else
            {
                return Guid.Empty;
            }
        }

        private void BuildChangeGraph(TItem entity)
        {
            DbContext.ChangeTracker.TrackGraph(entity, node =>
            {
                var entry = node.Entry;
                var childEntity = entry.Entity;

                if (entry.IsKeySet)
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    entry.State = EntityState.Added;
                }

            });
        }

        public async Task<TItem> AddSlowAsync(TItem entity)
        {
            await Task.Delay(2500);
            return await AddAsync(entity).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync(Dictionary<string, string> filterValues)
        {
            return await ApplyFiltering(DbSet, filterValues).CountAsync().ConfigureAwait(false);
        }


    }

    public static class EFExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
