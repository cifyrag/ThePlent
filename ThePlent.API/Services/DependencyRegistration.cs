using ThePlant.API.Services.Interfaces;
using ThePlant.API.Services.Realisations;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
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

        services.AddScoped<IAchievementsService, AchievementsService>();
        services.AddScoped<IFeedBacksService, FeedBacksService>();
        services.AddScoped<IPlantCareInstructionsService, PlantCareInstructionsService>();
        services.AddScoped<IPlantOverviewService, PlantOverviewService>();
        services.AddScoped<IPlantService, PlantService>();
        services.AddScoped<IRemindersService, RemindersService>();
        services.AddScoped<ISubscriptionsService, SubscriptionsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ІUserGardenService, UserGardenService>();
        services.AddScoped<IUserPlantService, UserPlantService>();
        services.AddScoped(typeof(IGenericRepository<PlantImage>), typeof(GenericRepository<PlantImage>));
        services.AddScoped<IPlantService, PlantService>();
        services.AddScoped<IPlantImagesService, PlantImagesService>();



        return services;
    }
}