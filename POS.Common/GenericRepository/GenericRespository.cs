using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POS.Common.UnitOfWork;
using POS.Data;
using System.Threading.Tasks;

namespace POS.Common.GenericRepository
{
    public class GenericRepository<TC, TContext> : IGenericRepository<TC>
        where TC : class
        where TContext : DbContext
    {
        protected readonly TContext Context;
        internal readonly DbSet<TC> DbSet;
        protected IUnitOfWork<TContext> _uow;
        protected GenericRepository(IUnitOfWork<TContext> uow
            )
        {
            Context = uow.Context;
            this._uow = uow;
            DbSet = Context.Set<TC>();
        }
        public IQueryable<TC> All => Context.Set<TC>();
        public void Add(TC entity)
        {
            Context.Add(entity);
        }

        public IQueryable<TC> AllIncluding(params Expression<Func<TC, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties);
        }

        //public async Task<IEnumerable<TC>> AllIncludingAsync(params Expression<Func<TC, object>>[] includeProperties)
        //{
        //    var query = GetAllIncluding(includeProperties);
        //    IEnumerable<TC> results = await query.ToListAsync();
        //    return results;
        //}

        public IQueryable<TC> FindByInclude(Expression<Func<TC, bool>> predicate, params Expression<Func<TC, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            return query.Where(predicate);
        }

        //public async Task<IEnumerable<TC>> FindByIncludeAsync(Expression<Func<TC, bool>> predicate, params Expression<Func<TC, object>>[] includeProperties)
        //{
        //    var query = GetAllIncluding(includeProperties);
        //    IEnumerable<TC> results = await query.Where(predicate).ToListAsync();
        //    return results;
        //}

        //public  IQueryable<TC> FindByInclude(Expression<Func<TC, bool>> predicate, params Expression<Func<TC, object>>[] includeProperties)
        //{
        //    var query = GetAllIncluding(includeProperties);
        //    return query.Where(predicate);
        //}

        public IQueryable<TC> FindBy(Expression<Func<TC, bool>> predicate)
        {
            IQueryable<TC> queryable = DbSet.AsNoTracking();
            return queryable.Where(predicate);
        }

        //public IQueryable<TC> FindOnly(Expression<Func<TC, bool>> predicate)
        //{
        //    IQueryable<TC> queryable = DbSet.AsNoTracking();
        //    return queryable.Where(predicate);
        //}

        //public async Task<IEnumerable<TC>> FindByAsync(Expression<Func<TC, bool>> predicate)
        //{
        //    IQueryable<TC> queryable = DbSet.AsNoTracking();
        //    IEnumerable<TC> results = await queryable.Where(predicate).ToListAsync();
        //    return results;
        //}

        private IQueryable<TC> GetAllIncluding(params Expression<Func<TC, object>>[] includeProperties)
        {
            IQueryable<TC> queryable = DbSet.AsNoTracking();

            return includeProperties.Aggregate
              (queryable, (current, includeProperty) => current.Include(includeProperty));
        }
        public TC Find(Guid id)
        {
            return Context.Set<TC>().Find(id);
        }

        public async Task<TC> FindAsync(Guid id)
        {
            return await Context.Set<TC>().FindAsync(id);
        }

        public virtual void Update(TC entity)
        {
            Context.Update(entity);
        }
        public virtual void UpdateRange(List<TC> entities)
        {
            Context.UpdateRange(entities);
        }

        public void RemoveRange(IEnumerable<TC> lstEntities)
        {
            Context.Set<TC>().RemoveRange(lstEntities);
        }

        public void AddRange(IEnumerable<TC> lstEntities)
        {
            Context.Set<TC>().AddRange(lstEntities);
        }

        public void InsertUpdateGraph(TC entity)
        {
            Context.Set<TC>().Add(entity);
            //Context.ApplyStateChanges(user);
        }
        public virtual void Delete(Guid id)
        {
            var entity = Context.Set<TC>().Find(id) as BaseEntity;
            if (entity != null)
            {
                entity.IsDeleted = true;
                Context.Update(entity);
            }
        }
        public virtual void Delete(TC entityData)
        {
            var entity = entityData as BaseEntity;
            entity.IsDeleted = true;
            Context.Update(entity);
        }
        public virtual void Remove(TC entity)
        {
            Context.Remove(entity);
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
