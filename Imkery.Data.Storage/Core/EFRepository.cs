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
using FluentValidation;
using Imkery.Entities;
using System.Collections;

namespace Imkery.Data.Storage.Core
{
    public abstract class EFRepository
    {
        public ImkeryDbContext DbContext
        {
            get; set;
        }

        public virtual Task<bool> CheckIfMemberMayChangeObject(Guid guid, IImkeryUser user)
        {
            return Task.FromResult(false);
        }


        public virtual Task<bool> CheckIfMemberMayDeleteObject(Guid guid, IImkeryUser user)
        {
            return Task.FromResult(false);
        }

        internal abstract IValidator GetValidatorAbstract();

    }
    public abstract class EFRepository<T> : EFRepository where T : class, IEntity<T>, new()
    {
        public bool SeperatePerUser { get; set; }
        public EFRepository(ImkeryDbContext dbContext, IImkeryUserProvider userProvider)
        {
            DbContext = dbContext;
            UserProvider = userProvider;
        }
        public abstract void ConfigureModel(EntityTypeBuilder<T> modelBuilder);


        public static void ChangeDbContext(ImkeryDbContext databaseContext)
        {

        }

        public abstract DbSet<T> DbSet { get; }
        public IImkeryUserProvider UserProvider { get; }

        public virtual async Task<T> AddAsync(T entity)
        {
            var user = await UserProvider.GetCurrentUserAsync();
            if (user != null)
            {
                entity.OwnerId = user.GuidId;
            }
            BuildChangeGraph(entity);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            UntrackItem(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            BeforeItemDeleted(entity);
            //DbContext.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        public virtual void BeforeItemDeleted(T entity)
        {

        }
        public abstract IQueryable<T> ApplyFiltering(IQueryable<T> query, Dictionary<string, string> filterValues);

        public virtual async Task<ICollection<T>> GetCollectionAsync(int from, int count, string sortField, bool desc, Dictionary<string, string> filterValues, string[] includes)
        {
            IQueryable<T> collection = DbSet;
            if (SeperatePerUser)
            {
                var currentUser = await UserProvider.GetCurrentUserAsync();
                if (currentUser != null)
                {
                    collection = collection.Where(b => b.OwnerId == currentUser.GuidId);
                }
                else
                {
                    collection = collection.Where(b => false);
                }
            }
            collection = ApplyFiltering(collection, filterValues);
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

        public virtual async Task<T> GetItemByIdAsync(Guid id)
        {
            return await GetItemById(id, null);
        }

        public virtual async Task<T> GetItemById(Guid id, string[] includes)
        {
            IQueryable<T> collection = DbSet.AsNoTracking();
            if (SeperatePerUser)
            {
                var currentUser = await UserProvider.GetCurrentUserAsync();
                if (currentUser != null)
                {
                    collection = collection.Where(b => b.OwnerId == currentUser.GuidId);
                }
                else
                {
                    collection = collection.Where(b => false);
                }
            }
            if (includes == null || includes.Length == 0)
            {
                return await collection.FirstOrDefaultAsync(b => b.Id == id);
            }
            else
            {
                foreach (string include in includes)
                {
                    collection = collection.Include(include);
                }
                var item = await collection.FirstOrDefaultAsync(b => b.Id == id);
                if (item == null)
                {
                    return null;
                }

                return item;
            }

        }

        public virtual void CancelEdit(T entity)
        {
            UntrackItem(entity);
        }

        public virtual void UntrackItem(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
            var entries = DbContext.ChangeTracker.Entries().ToList();
            foreach (EntityEntry dbEntityEntry in entries)
            {
                dbEntityEntry.State = EntityState.Detached;
            }
        }

        public virtual async Task<T> UpdateAsync(Guid id, T entity)
        {
            BuildChangeGraph(entity);
            foreach (var collection in DbContext.Entry(entity).Collections.ToList())
            {
                var loadedEntity = await DbSet.Include(collection.Metadata.Name).AsNoTracking().FirstOrDefaultAsync(b => b.Id == entity.Id);

                var dbCollection = (loadedEntity.GetType().GetProperty(collection.Metadata.Name).GetValue(loadedEntity) as IEnumerable<object>).ToList();
                var currenentValues = collection.CurrentValue.Cast<object>();
                foreach (var itemInDb in dbCollection)
                {

                    if (currenentValues.Where(b => GetKey(b) == GetKey(itemInDb)).Count() == 0)
                    {
                        DbContext.Remove(itemInDb);
                    }
                }
            }
            await DbContext.SaveChangesAsync();
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual Guid GetKey<T>(T entity)
        {
            var keyName = DbContext.Model.FindEntityType(entity.GetType()).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
            return (Guid)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }

        private void BuildChangeGraph(T entity)
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

        public async Task<int> GetCountAsync(Dictionary<string, string> filterValues)
        {
            return await ApplyFiltering(DbSet, filterValues).CountAsync().ConfigureAwait(false);
        }

        public override async Task<bool> CheckIfMemberMayDeleteObject(Guid guid, IImkeryUser user)
        {
            var item = await DbSet.FindAsync(guid).ConfigureAwait(false);
            if (item == null)
            {
                return false;
            }
            return item.OwnerId == user.GuidId;
        }


        public override async Task<bool> CheckIfMemberMayChangeObject(Guid guid, IImkeryUser user)
        {
            var item = await DbSet.FindAsync(guid).ConfigureAwait(false);
            if (item == null)
            {
                return false;
            }
            return item.OwnerId == user.GuidId;
        }

        public virtual AbstractValidator<T> GetValidator()
        {
            return new T().GetValidator();
        }

        internal override IValidator GetValidatorAbstract() => GetValidator();
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
