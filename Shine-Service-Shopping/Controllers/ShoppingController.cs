using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shine_Service_Shopping.Database;
using Shine_Service_Shopping.Models;

namespace Shine_Service_Shopping.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingController(ILogger<ShoppingController> logger, AppDbContext context) : ControllerBase
{
    private readonly ILogger<ShoppingController> _logger = logger;
    private readonly AppDbContext _context = context;

    [HttpGet("get")]
    public async Task<IActionResult> GetShoppingLists()
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        ArgumentNullException.ThrowIfNull(userId);

        var shoppingLists = await _context.ShoppingLists.Where(s => s.UserId == userId).ToListAsync();

        return Ok(shoppingLists);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddShoppingList([FromBody] ShoppingListModel model)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            ArgumentNullException.ThrowIfNull(userId);

            var shoppingList = new ShoppingList { Id = model.Id, Name = model.Name, UserId = userId, CreatedAt = DateTime.Now };

            _context.ShoppingLists.Add(shoppingList);

            await _context.SaveChangesAsync();

            return Ok(new ShoppingListAnswer
            {
                Id = shoppingList.Id,
                Name = shoppingList.Name,
                UserId = shoppingList.UserId,
                CreatedAt = shoppingList.CreatedAt
            });
        }
        catch (Exception ex)
        {
            var message = "Error to create shopping list.";

            _logger.LogError(ex, message);

            return BadRequest(message);
        }
    }

    [HttpGet("getitems")]
    public async Task<IActionResult> GetShoppingListItems(string shoppingListId)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        ArgumentNullException.ThrowIfNull(userId);

        var shoppingList = await _context.ShoppingLists.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == shoppingListId);

        if (shoppingList == null)
        {
            return BadRequest($"Shopping list '{shoppingListId}' is not for user '{userId}'!");
        }

        var shoppingListItems = _context.ShoppingListItems.Where(s => s.ShoppingListId == shoppingListId);

        return Ok(shoppingListItems);
    }

    [HttpPost("additem")]
    public async Task<IActionResult> AddShoppingListItem([FromBody] ShoppingListItemModel model)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            ArgumentNullException.ThrowIfNull(userId);

            var shoppingListItem = new ShoppingListItem
            {
                Id = model.Id,
                ShoppingListId = model.ShoppingListId,
                Name = model.Name,
                IsPurchased = false,
                CreatedAt = DateTime.Now
            };

            _context.ShoppingListItems.Add(shoppingListItem);

            await _context.SaveChangesAsync();

            return Ok(shoppingListItem);
        }
        catch (Exception ex)
        {
            var message = "Error to create shopping list item.";

            _logger.LogError(ex, message);

            return BadRequest(message);
        }
    }

    [HttpPost("removeitem")]
    public async Task<IActionResult> RemoveShoppingListItem([FromBody] string shoppingItemId)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            ArgumentNullException.ThrowIfNull(userId);
            
            var shoppingListItem = await _context.ShoppingListItems.FirstOrDefaultAsync(item => item.Id == shoppingItemId);

            if (shoppingListItem is null)
            {
                return NotFound("Shopping list item not found.");
            }

            _context.ShoppingListItems.Remove(shoppingListItem);
            
            await _context.SaveChangesAsync();

            return Ok(shoppingListItem);
        }
        catch (Exception ex)
        {
            var message = "Error to remove shopping list item.";

            _logger.LogError(ex, message);

            return BadRequest(message);
        }
    }

    [HttpPost("setispurchaseditem")]
    public async Task<IActionResult>  SetIsPurchasedShoppingListItem([FromBody] IsPurchasedShoppingListItemModel model)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            ArgumentNullException.ThrowIfNull(userId);
            
            var shoppingListItem = await _context.ShoppingListItems.FirstOrDefaultAsync(item => item.Id == model.Id);

            if (shoppingListItem is null)
            {
                return NotFound("Shopping list item not found.");
            }

            shoppingListItem.IsPurchased = model.Value;
            
            await _context.SaveChangesAsync();

            return Ok(shoppingListItem);
        }
        catch (Exception ex)
        {
            var message = "Error to change purchased shopping list item.";

            _logger.LogError(ex, message);

            return BadRequest(message);
        }
    }
}