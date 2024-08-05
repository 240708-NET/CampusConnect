namespace BlogAPI.Models;

using Microsoft.EntityFrameworkCore;

public class BlogContext(DbContextOptions<BlogContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
