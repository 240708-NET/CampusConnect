namespace BlogAPI.Repositories;

using BlogAPI.Models;

public class UserRepository(BlogContext context) : GenericRepository<User>(context) { }
