using System.Linq.Expressions;

namespace Gacoan.Backend.Repository;

public interface IRepository<T> where T : class
{
    // C
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    
    // R
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    IQueryable<T> GetQueryable();
    
    // U
    Task UpdateAsync(Guid id, T updatedEntity, CancellationToken cancellationToken = default);
    
    // D
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
