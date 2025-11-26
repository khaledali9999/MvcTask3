using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);

    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default
    );

    Task<T?> GetOneAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true,
        CancellationToken cancellationToken = default
    );

    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        List<Expression<Func<T, object>>>? includes = null,
        CancellationToken cancellationToken = default
    );

    Task CommitAsync(CancellationToken cancellationToken = default);
}
