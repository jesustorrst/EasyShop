using EasyShop.Identity.Application.Interfaces;
using EasyShop.Identity.Infrastructure.Services;
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
        services.AddDataProtection();

        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityConnectionString"),
                b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })

        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}