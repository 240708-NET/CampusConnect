namespace BlogAPI.Repositories;

using BlogAPI.Models;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<List<Comment>?> GetChildCommentsByParentID(int parentID);
}
