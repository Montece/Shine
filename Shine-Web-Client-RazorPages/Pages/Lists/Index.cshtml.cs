using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shine.Web.Client.Models;
using Shine.Web.Client.Services;
using System.ComponentModel.DataAnnotations;

namespace Shine.Web.Client.Pages.Lists;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IShoppingListService _shoppingListService;
    private readonly IAuthService _authService;

    public IndexModel(IShoppingListService shoppingListService, IAuthService authService)
    {
        _shoppingListService = shoppingListService;
        _authService = authService;
    }

    public List<ShoppingListViewModel>? Lists { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Название списка обязательно")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string NewListName { get; set; } = string.Empty;

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateListAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return Page();
        }

        var authToken = _authService.GetStoredToken(User);
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToPage("/Auth/Login");
        }

        var result = await _shoppingListService.CreateShoppingListAsync(NewListName, authToken);

        if (result != null)
        {
            SuccessMessage = $"Список \"{NewListName}\" успешно создан!";
            return RedirectToPage();
        }

        ErrorMessage = "Не удалось создать список. Попробуйте еще раз.";
        await LoadListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteListAsync(string listId)
    {
        // TODO: Implement delete functionality when API supports it
        ErrorMessage = "Функция удаления списков пока не реализована.";
        return RedirectToPage();
    }

    private async Task LoadListsAsync()
    {
        var authToken = _authService.GetStoredToken(User);
        if (string.IsNullOrEmpty(authToken))
        {
            Lists = new List<ShoppingListViewModel>();
            return;
        }

        var allLists = await _shoppingListService.GetShoppingListsAsync(authToken);

        if (allLists != null)
        {
            Lists = string.IsNullOrEmpty(SearchTerm)
                ? allLists
                : allLists.Where(l => l.List.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        else
        {
            Lists = new List<ShoppingListViewModel>();
            ErrorMessage = "Не удалось загрузить списки покупок.";
        }
    }
}
