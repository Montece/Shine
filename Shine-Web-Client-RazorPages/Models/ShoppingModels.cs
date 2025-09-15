using System.ComponentModel.DataAnnotations;

namespace Shine.Web.Client.Models;

public class ShoppingList
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}

public class ShoppingListItem
{
    public string Id { get; set; } = string.Empty;
    public string ShoppingListId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsPurchased { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class CreateShoppingListModel
{
    [Required(ErrorMessage = "Название списка обязательно")]
    [Display(Name = "Название списка")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string Name { get; set; } = string.Empty;
}

public class CreateShoppingListItemModel
{
    [Required(ErrorMessage = "Название товара обязательно")]
    [Display(Name = "Название товара")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string Name { get; set; } = string.Empty;
    
    public string ShoppingListId { get; set; } = string.Empty;
}

public class ShoppingListViewModel
{
    public ShoppingList List { get; set; } = new();
    public List<ShoppingListItem> Items { get; set; } = new();
    public int TotalItems => Items.Count;
    public int CompletedItems => Items.Count(i => i.IsPurchased);
    public double Progress => TotalItems > 0 ? (double)CompletedItems / TotalItems * 100 : 0;
}

public class ShoppingListsViewModel
{
    public List<ShoppingListViewModel> Lists { get; set; } = new();
    public string SearchTerm { get; set; } = string.Empty;
}

// API Request/Response models
public class ShoppingListCreateRequest
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class ShoppingListItemCreateRequest
{
    public string Id { get; set; } = string.Empty;
    public string ShoppingListId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class ShoppingListItemUpdateRequest
{
    public string Id { get; set; } = string.Empty;
    public bool Value { get; set; }
}
