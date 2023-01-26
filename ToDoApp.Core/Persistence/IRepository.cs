using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ToDoApp.Core.Persistence
{
    public interface IRepository<TEntity, in TKey>: IDisposable where TEntity : class
    {
        TEntity Get(TKey id);
        Task<TEntity> GetAsnyc(TKey id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Save(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TKey id);
    }
}
