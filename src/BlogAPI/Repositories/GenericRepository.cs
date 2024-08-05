namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<TEntity>(BlogContext context) where TEntity : class
{
    protected BlogContext Context = context;
    protected DbSet<TEntity> EntitySet = context.Set<TEntity>();

    public virtual Task<List<TEntity>> Get()
    {
        return EntitySet.ToListAsync();
    }

    public virtual ValueTask<TEntity?> GetById(object id)
    {
        return EntitySet.FindAsync(id);
    }

    public virtual async Task Insert(TEntity entity)
    {
        await EntitySet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task Update(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    public virtual async Task Delete(TEntity entity)
    {
        EntitySet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
