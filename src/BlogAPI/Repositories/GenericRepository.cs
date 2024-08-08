namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<TEntity>(DbContext context) : IGenericRepository<TEntity>
where TEntity : class, IIdentified
{
    protected DbContext Context = context;
    protected DbSet<TEntity> EntitySet = context.Set<TEntity>();

    public virtual Task<List<TEntity>> Get()
    {
        return EntitySet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetById(int id)
    {
        return await EntitySet.AsNoTracking().SingleOrDefaultAsync(e => e.ID == id);
    }

    public virtual async Task Insert(TEntity entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<bool> Update(TEntity entity)
    {
        Context.Update(entity);
        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EntitySet.Any(e => e.ID == entity.ID)) return false;
            throw;
        }
        return true;
    }

    public virtual async Task<bool> DeleteById(object id)
    {
        var entity = await EntitySet.FindAsync(id);
        if (entity is null) return false;
        Context.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }
}
