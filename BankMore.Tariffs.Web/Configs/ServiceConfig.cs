using BankMore.Tariffs.Infrastructure;

namespace BankMore.Tariffs.Web.Configs;

public static class ServiceConfigs
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection services, ILogger logger,
        WebApplicationBuilder builder)
    {
        services.AddInfrastructure(builder.Configuration, logger);

        logger.LogInformation("services registered");

        return services;
    }
}