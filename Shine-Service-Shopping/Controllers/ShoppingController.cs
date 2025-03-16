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
        var userId = HttpContext.Items["UserId"]?.ToString();
        ArgumentNullException.ThrowIfNull(userId);

        var shoppingList = new ShoppingList
        {
            Id = model.Id,
            Name = model.Name,
            UserId = userId,
            CreatedAt = DateTime.Now
        };

        _context.ShoppingLists.Add(shoppingList);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(AddShoppingList), new { id = model.Id }, model);
    }
}