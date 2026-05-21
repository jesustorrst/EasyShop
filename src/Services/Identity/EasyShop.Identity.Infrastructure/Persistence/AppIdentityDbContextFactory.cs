using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyShop.Identity.Infrastructure.Persistence;

public class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
{
    public AppIdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();

        var connectionString = "Server=localhost,1433;Database=EasyShopIdentityDb;User Id=sa;Password=Qwerty123456;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString,
            b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName));

        return new AppIdentityDbContext(optionsBuilder.Options);
    }
}