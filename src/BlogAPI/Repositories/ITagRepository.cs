namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<List<Post>?> GetPostsByTagID(int tagID);
}
