using Kyon.Dolphin.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Kyon.Dolphin.Core.Data.Repositories;

public abstract class MinimalRepository<TEntity, TKey, TDbContext> : IMinimalRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;

    public MinimalRepository(TDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public virtual async Task SaveAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        var existingEntity = await _dbContext.Set<TEntity>().FindAsync(entity.Id);

        if (existingEntity != null)
        {
            _dbContext.Entry(existingEntity).State = EntityState.Detached;
            await _dbContext.SaveChangesAsync();
        }

        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> RetrieveAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> RetrieveByIdAsync(object id)
    {
        # pragma warning disable CS8603

        switch (id)
        {

            case int intId:
                return await _dbContext.Set<TEntity>().FindAsync(intId);
            case string stringId:
                return await _dbContext.Set<TEntity>().FindAsync(stringId);
            default:
                throw new ArgumentException("Unsupported key type");
        }

        # pragma warning restore
    }
}