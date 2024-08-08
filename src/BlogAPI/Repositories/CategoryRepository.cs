namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository(BlogContext context) : GenericRepository<Category>(context), ICategoryRepository
{
    public async Task<List<Post>?> GetPostsByCategoryID(int categoryID)
    {
        if (await context.Categories.FindAsync(categoryID) is null) return null;
        return await (from post in context.Posts where post.CategoryID == categoryID select post).ToListAsync();
    }
}
