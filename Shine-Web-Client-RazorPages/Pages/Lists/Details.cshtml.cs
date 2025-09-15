using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shine.Web.Client.Models;
using Shine.Web.Client.Services;
using System.ComponentModel.DataAnnotations;

namespace Shine.Web.Client.Pages.Lists;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IShoppingListService _shoppingListService;
    private readonly IAuthService _authService;

    public DetailsModel(IShoppingListService shoppingListService, IAuthService authService)
    {
        _shoppingListService = shoppingListService;
        _authService = authService;
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; } = string.Empty;

    public ShoppingListViewModel? ListViewModel { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Название товара обязательно")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string NewItemName { get; set; } = string.Empty;

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return NotFound();
        }

        await LoadListAsync();
        
        if (ListViewModel == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddItemAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListAsync();
            return Page();
        }

        var authToken = _authService.GetStoredToken(User);
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToPage("/Auth/Login");
        }

        var result = await _shoppingListService.AddShoppingListItemAsync(Id, NewItemName, authToken);

        if (result != null)
        {
            SuccessMessage = $"Товар \"{NewItemName}\" добавлен в список!";
            return RedirectToPage(new { Id });
        }

        ErrorMessage = "Не удалось добавить товар. Попробуйте еще раз.";
        await LoadListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostToggleItemAsync(string itemId, bool isPurchased)
    {
        var authToken = _authService.GetStoredToken(User);
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToPage("/Auth/Login");
        }

        var success = await _shoppingListService.ToggleItemPurchasedAsync(itemId, isPurchased, authToken);

        if (success)
        {
            SuccessMessage = isPurchased ? "Товар отмечен как купленный!" : "Товар отмечен как не купленный!";
        }
        else
        {
            ErrorMessage = "Не удалось изменить статус товара.";
        }

        return RedirectToPage(new { Id });
    }

    public async Task<IActionResult> OnPostRemoveItemAsync(string itemId)
    {
        var authToken = _authService.GetStoredToken(User);
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToPage("/Auth/Login");
        }

        var success = await _shoppingListService.RemoveShoppingListItemAsync(itemId, authToken);

        if (success)
        {
            SuccessMessage = "Товар удален из списка!";
        }
        else
        {
            ErrorMessage = "Не удалось удалить товар.";
        }

        return RedirectToPage(new { Id });
    }

    private async Task LoadListAsync()
    {
        var authToken = _authService.GetStoredToken(User);
        if (string.IsNullOrEmpty(authToken))
        {
            ListViewModel = null;
            return;
        }

        var lists = await _shoppingListService.GetShoppingListsAsync(authToken);
        ListViewModel = lists?.FirstOrDefault(l => l.List.Id == Id);

        if (ListViewModel == null)
        {
            ErrorMessage = "Не удалось загрузить список покупок.";
        }
    }
}
