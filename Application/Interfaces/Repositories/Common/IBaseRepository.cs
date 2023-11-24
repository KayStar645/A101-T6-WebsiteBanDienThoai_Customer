using Domain.Common.Interfaces;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories.Common
{
    public interface IBaseRepository<T> where T : class, IEntity
    {
        // Async
        Task<List<T>> GetAllAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int? id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task Save();

        // Query
        IQueryable<T> Entities { get; }
        IQueryable<T> Query();
        IQueryable<T> GetAllInclude(Expression<Func<T, object>> includeProperties = null);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetByIdInclude(int? id, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> AddInclude(IQueryable<T> query, Expression<Func<T, object>> includeProperties = null);
    }
}
