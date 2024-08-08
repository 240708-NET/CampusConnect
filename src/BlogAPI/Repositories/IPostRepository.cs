namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface IPostRepository : IGenericRepository<Post>
{
    Task<List<Tag>> GetTagsById(int id);
}
