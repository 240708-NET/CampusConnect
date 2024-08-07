namespace BlogAPI.Repositories;

using BlogAPI.Models;

public class TagRepository(BlogContext context) : GenericRepository<Tag>(context), ITagRepository { }
