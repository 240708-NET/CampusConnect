namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class TagRepository(BlogContext context) : GenericRepository<Tag>(context), ITagRepository
{
    public async Task<List<Post>?> GetPostsByTagID(int tagID)
    {
        if (await context.Tags.FindAsync(tagID) is null) return null;
        return await (from postTag in context.PostTags where postTag.TagID == tagID select postTag.Post).ToListAsync();
    }
}
