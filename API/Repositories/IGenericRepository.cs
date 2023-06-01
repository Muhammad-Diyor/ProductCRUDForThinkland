using System.Linq.Expressions;

namespace API.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    TEntity? GetById(Guid id);
    IQueryable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    ValueTask<TEntity> AddAsync(TEntity entity);
    ValueTask<TEntity> RemoveAsync(TEntity entity);
    ValueTask RemoveRangeAsync(IEnumerable<TEntity> entities);
    ValueTask<TEntity> UpdateAsync(TEntity entity);
}