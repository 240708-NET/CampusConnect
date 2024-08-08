namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class CommentRepository(BlogContext context) : GenericRepository<Comment>(context), ICommentRepository
{
    public async Task<List<Comment>?> GetChildCommentsByParentID(int parentID)
    {
        if (await context.Comments.FindAsync(parentID) is null) return null;
        return await (from comment in context.Comments where comment.ParentCommentID == parentID select comment).ToListAsync();
    }
}
