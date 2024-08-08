namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class PostRepository(BlogContext context) : GenericRepository<Post>(context), IPostRepository
{
    public Task<List<Tag>> GetTagsById(int id)
    {
        return (from postTag in context.PostTags where postTag.PostID == id select postTag.Tag).ToListAsync();
    }
}
