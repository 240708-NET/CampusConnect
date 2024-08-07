namespace BlogAPI.Repositories;

using BlogAPI.Models;

public class PostRepository(BlogContext context) : GenericRepository<Post>(context), IPostRepository { }
