using System.Linq.Expressions;
using API.Data;

namespace API.Repositories;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async ValueTask<TEntity> AddAsync(TEntity entity)
    {
        var entry = await _context.Set<TEntity>().AddAsync(entity);

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        => _context.Set<TEntity>().Where(expression);

    public IQueryable<TEntity> GetAll()
        => _context.Set<TEntity>();

    public TEntity? GetById(ulong id)
        => _context.Set<TEntity>().Find(id);

    public async ValueTask<TEntity> RemoveAsync(TEntity entity)
    {
        var entry = _context.Set<TEntity>().Remove(entity);

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public async ValueTask<TEntity> UpdateAsync(TEntity entity)
    {
        var entry = _context.Set<TEntity>().Update(entity);

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public async ValueTask RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);

        await _context.SaveChangesAsync();
    }
}
