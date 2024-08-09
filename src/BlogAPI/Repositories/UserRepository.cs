namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository(BlogContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<List<Post>?> GetPostsByUserID(int userID)
    {
        if (await context.Users.FindAsync(userID) is null) return null;
        return await (from post in context.Posts where post.PosterID == userID select post).ToListAsync();
    }

    public async Task<List<Comment>?> GetCommentsByUserID(int userID)
    {
        if (await context.Users.FindAsync(userID) is null) return null;
        return await (from comment in context.Comments where comment.CommenterID == userID select comment).ToListAsync();
    }
}