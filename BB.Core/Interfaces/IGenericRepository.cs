using System.Linq.Expressions;

namespace BB.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Retrieve a single entity by its primary key ID.
        T GetById(int? id);

        // Retrieve a single entity matching the given predicate.
        // <param name="predicate">Expression used as a filter (similar to SQL WHERE clause).</param>
        // <param name="trackChanges">Whether to enable EF change tracking (defaults to false for performance).</param>
        // <param name="includes">Comma-separated related entities to include (similar to SQL JOIN).</param>
        T Get(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null);

        // Retrieve a collection of entities with optional filtering, ordering, and joining.
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, int>>? orderBy = null,
            string? includes = null);
        // <param name="predicate">Optional filtering expression.</param>
        // <param name="orderBy">Optional ordering expression.</param>
        // <param name="includes">Optional navigation properties to include.</param>

        //Asynchronously retrieve a collection of entities with optional filtering, ordering, and eager loading.
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, int>>? orderBy = null,
            string? includes = null);

        // Insert a new entity.
        void Add(T entity);
        // Remove a single entity.
        void Delete(T entity);

        // Remove multiple entities.
        void Delete(IEnumerable<T> entities);

        // Update an existing entity.
        void Update(T entity);

    }
}

