namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<List<Post>?> GetPostsByCategoryID(int categoryID);
}
