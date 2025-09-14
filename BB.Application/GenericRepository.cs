using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BB.Core.Interfaces;
using BB.Infrastructure.Data;

namespace BB.Application
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
            _db.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            _db.Set<T>().RemoveRange(entities);
            _db.SaveChanges();
        }

        public virtual T GetById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = _db.Set<T>().Find(id.Value);
            return entity ?? throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} not found.");
        }

        public virtual T Get(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null)
        {
            return BuildQuery(predicate, trackChanges, includes).FirstOrDefault()
                   ?? throw new InvalidOperationException($"Entity of type {typeof(T).Name} matching the predicate was not found.");
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null)
        {
            return await BuildQuery(predicate, trackChanges, includes).FirstOrDefaultAsync()
                   ?? throw new InvalidOperationException($"Entity of type {typeof(T).Name} matching the predicate was not found.");
        }

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, int>>? orderBy = null,
            string? includes = null)
        {
            return BuildQuery(predicate, orderBy, includes).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, int>>? orderBy = null,
            string? includes = null)
        {
            return await BuildQuery(predicate, orderBy, includes).ToListAsync();
        }

        public void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        private IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? predicate,
            bool trackChanges,
            string? includes)
        {
            IQueryable<T> query = _db.Set<T>();

            if (!trackChanges)
                query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(include.Trim());
            }

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        private IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, int>>? orderBy,
            string? includes)
        {
            var query = BuildQuery(predicate, trackChanges: true, includes);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return query;
        }
    }
}