using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;

namespace AutoFacMvc.Repository.Core
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public BaseRepository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> All()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where<TEntity>(predicate).AsQueryable<TEntity>();
        }

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0,
            int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null
                ? Context.Set<TEntity>().Where<TEntity>(filter).AsQueryable()
                : Context.Set<TEntity>().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        public virtual TEntity Find(params object[] keys)
        {
            return Context.Set<TEntity>().Find(keys);
        }

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault<TEntity>(predicate);
        }

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        public virtual void Create(TEntity t)
        {
            Context.Set<TEntity>().Add(t);
        }

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        public virtual void Delete(TEntity t)
        {
            Context.Set<TEntity>().Remove(t);
        }

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                Context.Set<TEntity>().Remove(obj);
            return Context.SaveChanges();
        }

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        public virtual void Update(TEntity t)
        {
            try
            {
                var entry = Context.Entry(t);
                Context.Set<TEntity>().Attach(t);
                entry.State = EntityState.Modified;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Select Single Item by specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return All().FirstOrDefault(expression);
        }
    }
}
