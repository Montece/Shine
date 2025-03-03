using Microsoft.EntityFrameworkCore;

namespace Shine_Service_Shopping;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingList> ShoppingLists { get; set; }
}