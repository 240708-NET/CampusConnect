namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface IUserRepository : IGenericRepository<User>
{
    Task<List<Post>?> GetPostsByUserID(int userID);
    Task<List<Comment>?> GetCommentsByUserID(int userID);
}
