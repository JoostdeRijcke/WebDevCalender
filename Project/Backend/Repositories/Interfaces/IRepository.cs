using System.Linq.Expressions;

namespace CalendifyApp.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Basic CRUD Operations
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(T entity);

        // Advanced Query Operations
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Include related entities
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);

        // Save changes (per repository - useful for single operations)
        Task<int> SaveChangesAsync();
    }
}
