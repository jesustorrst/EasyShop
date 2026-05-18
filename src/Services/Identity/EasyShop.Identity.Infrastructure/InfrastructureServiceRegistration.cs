using EasyShop.Identity.Domain.Entities;
using EasyShop.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyShop.Identity.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityConnectionString"),
                b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));

        // 2. Configurar las reglas y servicios nativos de ASP.NET Core Identity
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            // Reglas de contraseñas seguras para tus usuarios
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;

            // Regla de correos únicos
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}