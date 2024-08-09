namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface IGenericRepository<TEntity>
{
    Task<List<TEntity>> Get();
    Task<TEntity?> GetById(int id);
    Task Insert(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> DeleteById(object id);
}
