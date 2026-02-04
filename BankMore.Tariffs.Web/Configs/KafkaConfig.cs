namespace BankMore.Tariffs.Web.Configs;

public static class KafkaConfig
{
    public static IServiceCollection AddKafkaConfig(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var kafkaBroker = configuration["Kafka:Brokers"];
        var kafkaTopic = configuration["Kafka:CompletedTransfersTopic"];
        
        if (string.IsNullOrWhiteSpace(kafkaBroker))
        {
            logger.LogWarning("Kafka broker is not configured.");
        }

        if (string.IsNullOrWhiteSpace(kafkaTopic))
        {
            logger.LogWarning("Kafka topic is not configured.");
        }

        logger.LogInformation("Kafka configuration loaded - Broker: {Broker}, Topic: {Topic}",
            kafkaBroker, kafkaTopic);

        return services;
    }
}
