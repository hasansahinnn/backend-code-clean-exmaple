using Core.Data.Repositories;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

/// <summary>
/// Extension methods for registering data services in the DI container.
/// </summary>
public static class DataServicesRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DBConnectionString")
                ?? throw new InvalidOperationException());
        });

        // Register repositories
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));


        return services;
    }
}