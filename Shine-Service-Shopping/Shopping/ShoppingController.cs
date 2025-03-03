using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Shine_Service_Shopping.Shopping;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ShoppingController(ILogger<ShoppingController> logger, AppDbContext context) : ControllerBase
{
    private readonly ILogger<ShoppingController> _logger = logger;
    private readonly AppDbContext _context = context;

    [HttpGet("get")]
    public async Task<IActionResult> GetShoppingLists()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var shoppingLists = await _context.ShoppingLists.Where(s => s.UserId == userId).ToListAsync();

        return Ok(shoppingLists);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddShoppingList([FromBody] ShoppingList shoppingList)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        shoppingList.UserId = userId;
        
        _context.ShoppingLists.Add(shoppingList);
        
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(AddShoppingList), new { id = shoppingList.Id }, shoppingList);
    }
}