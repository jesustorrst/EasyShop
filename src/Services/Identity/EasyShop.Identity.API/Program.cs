using EasyShop.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using EasyShop.Identity.Infrastructure;
using EasyShop.Identity.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentityInfrastructure(builder.Configuration);
// builder.Services.AddApplicationServices();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        await AppIdentityDbContextSeed.SeedAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al sembrar los usuarios por defecto.");
    }
}

app.Run();

