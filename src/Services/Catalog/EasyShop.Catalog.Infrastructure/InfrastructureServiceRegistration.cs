using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Infrastructure.Persistence;
using EasyShop.Catalog.Infrastructure.Repositories;
using EasyShop.Catalog.Infrastructure.Settings;
using EasyShop.Catalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MassTransit;
using EasyShop.Catalog.Application.Features.Products.EventConsumers;

namespace EasyShop.Catalog.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CatalogConnectionString")));



        services.Configure<MongoDbSettings>(options =>
        {
            var section = configuration.GetSection("MongoDbSettings");
            options.ConnectionString = section["ConnectionString"];
            options.DatabaseName = section["DatabaseName"];
        });


        services.AddSingleton(sp =>
        new MongoDbContext(sp.GetRequiredService<IOptions<MongoDbSettings>>()));

        //sql 
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

        //mongo
        services.AddScoped<IProductReadRepository, ProductReadRepository>();


        //services
        services.AddScoped<IProductWriteToReadService, ProductWriteToReadService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreatedConsumer>();
            x.AddConsumer<ProductUpdatedConsumer>();
            x.AddConsumer<ProductDeletedConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqUri = configuration["RabbitMq:Uri"];
                cfg.Host(new Uri(rabbitMqUri));

                cfg.ConfigureEndpoints(context);
            });
        });


        return services;
    }
}