using Microsoft.EntityFrameworkCore;

namespace Shine_Service_Users.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}