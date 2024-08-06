using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;

namespace BlogApi.Models;

public class BlogContext(DbContextOptions<BlogContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
