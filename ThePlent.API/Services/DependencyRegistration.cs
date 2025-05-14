using ThePlant.EF.Settings;

namespace ThePlant.API.Services;

public class DependencyRegistration
{
    public static IServiceCollection RegisterDependency(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWTSettings>(configuration.GetSection("Authentication:Schemes:Bearer"));
        services.Configure<GoogleBucketStorageSettings>(configuration.GetSection("Services:GoogleBucketStorage"));
        services.Configure<StripeSettings>(configuration.GetSection("Services:Stripe"));
        services.Configure<EmailSettings>(configuration.GetSection("Services:Email"));

        
        return services;
    }
}