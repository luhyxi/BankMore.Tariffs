using BankMore.Tariffs.Domain.Interfaces;
using BankMore.Tariffs.Infrastructure.Data;
using BankMore.Tariffs.Infrastructure.Messaging;
using BankMore.Tariffs.Infrastructure.Repositories;

namespace BankMore.Tariffs.Infrastructure;

public static class InfrastructureServiceInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var databaseSection = configuration.GetSection("Database");
        var connectionString = databaseSection.GetValue<string>("ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("Database connection string is not configured.");
        }

        services.Configure<DatabaseOptions>(databaseSection);
        services.AddScoped<IDbConnectionFactory, SqliteConnectionFactory>();

        services.AddScoped<ITariffRepository, TariffRepository>();

        services.AddKafkaMessaging(configuration);

        return services;
    }
}
