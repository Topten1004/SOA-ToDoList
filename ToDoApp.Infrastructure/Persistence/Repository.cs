using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDoApp.Core.Persistence;

namespace ToDoApp.Infrastructure.Persistence
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        protected Repository(IUnitOfWork<DbContext> unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork), @"Unit of work cannot be null");
            
            _dbSet = unitOfWork.DatabaseContext.Set<TEntity>();
        }

        public TEntity Get(TKey id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetAsnyc(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public TEntity Save(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public bool Update(TEntity entity)
        {
            _dbSet.AddOrUpdate(entity);
            return true;
        }

        public bool Delete(TKey id)
        {
            var entity = Get(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }

        protected TEntity SetEntityFields<T>(TEntity entity, TEntity from, params string[] properties)
        {
            var type = entity.GetType();
            
            foreach (string field in properties)
            {
                if (type.GetProperty(field) == null)
                    throw new ArgumentException();
                var propertyInfo = type.GetProperty(field);
                if (propertyInfo == null)
                    continue;

                propertyInfo.SetValue(entity, propertyInfo.GetValue(@from, null), null);
            }
            return entity;
        }

        public void Dispose()
        {
            //todo: bilare düzelt şunu
        }
    }
}
