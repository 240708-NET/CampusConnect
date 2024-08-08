namespace BlogAPI.Repositories;

using BlogAPI.Models;

public class PostTagRepository(BlogContext context) : GenericRepository<PostTag>(context), IPostTagRepository { }
