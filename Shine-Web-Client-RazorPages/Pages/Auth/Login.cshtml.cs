using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shine.Web.Client.Services;
using AuthModels = Shine.Web.Client.Models;

namespace Shine.Web.Client.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;

    public LoginModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public AuthModels.LoginModel LoginData { get; set; } = new();

    [TempData]
    public string? ErrorMessage { get; set; }

    [TempData]
    public string? SuccessMessage { get; set; }

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

        var success = await _authService.LoginAsync(LoginData);

        if (success)
        {
            return RedirectToPage("/Lists/Index");
        }

        ErrorMessage = "Неверный email или пароль. Попробуйте еще раз.";
        return Page();
    }
}
