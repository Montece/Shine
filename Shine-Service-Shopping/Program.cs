using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Shine_Service_Shopping.Database;
using Shine_Service_Shopping.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.WebHost.UseUrls("http://0.0.0.0:5000");

services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Postgres"));
});

services.AddAuthorization();

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Shine API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token with format: Bearer {your_token}"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(
        serviceName: "Shine-Service-Users",
        serviceVersion: "1.0.0"))
    .WithMetrics(m => m
        .AddAspNetCoreInstrumentation()   // RPS, latency, коды ответов
        .AddHttpClientInstrumentation()   // »сход€щие запросы
        .AddRuntimeInstrumentation()      // GC, потоки, аллокации
        .AddPrometheusExporter());

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{

}*/

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
}

app.MapPrometheusScrapingEndpoint();

//app.UseMiddleware<AuthMiddleware>();

app.UseWhen(
    ctx => !ctx.Request.Path.StartsWithSegments("/Ping/ping", StringComparison.OrdinalIgnoreCase) && !ctx.Request.Path.StartsWithSegments("/metrics", StringComparison.OrdinalIgnoreCase),
    branch => branch.UseMiddleware<AuthMiddleware>()
    );

if (Environment.GetEnvironmentVariable("INSTANCE_HEADER") == "true")
{
    app.Use(async (ctx, next) =>
    {
        ctx.Response.Headers["X-Instance"] = Environment.MachineName;
        await next();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();