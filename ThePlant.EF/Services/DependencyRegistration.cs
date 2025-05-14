using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThePlant.EF.Repository;

namespace ThePlant.EF.Services;

public class DependencyRegistration
{
    public static IServiceCollection RegisterDependency(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        return services;
    }
}