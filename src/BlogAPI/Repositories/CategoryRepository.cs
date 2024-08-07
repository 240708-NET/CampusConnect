namespace BlogAPI.Repositories;

using BlogAPI.Models;

public class CategoryRepository(BlogContext context) : GenericRepository<Category>(context), ICategoryRepository { }
