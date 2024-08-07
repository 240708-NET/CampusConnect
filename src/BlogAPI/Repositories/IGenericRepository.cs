namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface IGenericRepository<TEntity>
{
    Task<List<TEntity>> Get();
    ValueTask<TEntity?> GetById(object id);
    Task Insert(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> DeleteById(object id);
}
