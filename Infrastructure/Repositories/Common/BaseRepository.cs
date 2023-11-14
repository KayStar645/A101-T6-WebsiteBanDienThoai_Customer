using Application.Interfaces.Repositories.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Common
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private readonly SmartPhoneDbContext _context;

        public BaseRepository(SmartPhoneDbContext pContext)
        {
            _context = pContext;
        }

        // Async

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> ppPredicate)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(ppPredicate);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public virtual async Task<T> GetByIdAsync(int? id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Set<T>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public virtual async Task<T> AddAsync(T pEntity)
        {
            await _context.Set<T>().AddAsync(pEntity);
            return pEntity;
        }

        public virtual async Task<T> UpdateAsync(T pEntity)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            T exist = await _context.Set<T>()
                .Where(x => x.IsDeleted == false && x.Id == pEntity.Id)
                .FirstOrDefaultAsync();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            _context.Entry(exist).CurrentValues.SetValues(pEntity);
#pragma warning restore CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            return pEntity;
        }

        public virtual Task DeleteAsync(T pEntity)
        {
            _context.Set<T>().Remove(pEntity);
            return Task.CompletedTask;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> pPredicate)
        {
            return await _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .AnyAsync(pPredicate);
        }

        // Query
        public virtual IQueryable<T> Entities => _context.Set<T>();

        public virtual IQueryable<T> Query()
        {
            var query = _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking();

            return query;
        }

        public virtual IQueryable<T> GetAllInclude(Expression<Func<T, object>> pIncludeProperties = null)
        {
            var query = _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .AsNoTracking();

            if (pIncludeProperties != null)
            {
                query = query.Include(pIncludeProperties);
            }

            return query;
        }

        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> pPredicate)
        {
            return _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .Where(pPredicate);
        }

        public virtual IQueryable<T> GetByIdInclude(int? id, params Expression<Func<T, object>>[] pIncludeProperties)
        {
            var query = _context.Set<T>()
                .Where(x => x.IsDeleted == false)
                .AsQueryable();

            if (pIncludeProperties != null)
            {
                foreach (var includeProperty in pIncludeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (id != null)
            {
                query = query.Where(e => e.Id == id);
            }

            return query;
        }

        public virtual IQueryable<T> AddInclude(IQueryable<T> query, Expression<Func<T, object>> pIncludeProperties = null)
        {
            if (pIncludeProperties != null)
            {
                query = query.Include(pIncludeProperties);
            }
            return query;
        }
    }
}
