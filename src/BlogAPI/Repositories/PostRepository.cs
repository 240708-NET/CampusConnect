namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class PostRepository(BlogContext context) : GenericRepository<Post>(context), IPostRepository
{
    public async Task<List<Tag>?> GetTagsByPostID(int postID)
    {
        if (await context.Categories.FindAsync(postID) is null) return null;
        return await (from postTag in context.PostTags where postTag.PostID == postID select postTag.Tag).ToListAsync();
    }
}
