namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> Get();
    ValueTask<TEntity?> GetById(Object id);
    Task Insert(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
}
