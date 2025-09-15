using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shine.Web.Client.Services;
using AuthModels = Shine.Web.Client.Models;

namespace Shine.Web.Client.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly IAuthService _authService;

    public RegisterModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public AuthModels.RegisterModel RegisterData { get; set; } = new();

    [TempData]
    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        // Redirect if already authenticated
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Lists/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var success = await _authService.RegisterAsync(RegisterData);

        if (success)
        {
            TempData["SuccessMessage"] = "Регистрация прошла успешно! Теперь войдите в аккаунт.";
            return RedirectToPage("Login");
        }

        ErrorMessage = "Ошибка при регистрации. Возможно, пользователь с таким email уже существует.";
        return Page();
    }
}
