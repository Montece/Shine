using Microsoft.AspNetCore.Authentication.Cookies;
using Shine.Web.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure HttpClient for API calls
builder.Services.AddHttpClient<IUsersApiService, UsersApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:UsersApiUrl"] ?? "http://localhost:5000/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IShoppingApiService, ShoppingApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ShoppingApiUrl"] ?? "http://localhost:8080/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.Cookie.Name = "ShineAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddAuthorization();

// Session for temporary data
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Default redirect to lists page if authenticated, otherwise to login
app.MapGet("/", async context =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        context.Response.Redirect("/Lists");
    }
    else
    {
        context.Response.Redirect("/Auth/Login");
    }
});

app.Run();
