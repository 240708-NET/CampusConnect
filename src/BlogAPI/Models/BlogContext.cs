using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;

namespace BlogApi.Models;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
}
