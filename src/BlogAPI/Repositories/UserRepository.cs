namespace BlogAPI.Repositories;

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository(BlogContext context) : GenericRepository<User>(context), IUserRepository { }
