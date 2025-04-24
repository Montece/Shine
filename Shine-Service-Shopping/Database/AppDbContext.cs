using Microsoft.EntityFrameworkCore;

namespace Shine_Service_Shopping.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShoppingList> ShoppingLists { get; set; }

    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
}