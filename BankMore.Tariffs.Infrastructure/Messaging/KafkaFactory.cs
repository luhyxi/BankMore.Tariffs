using KafkaFlow;
using KafkaFlow.Serializer;

namespace BankMore.Tariffs.Infrastructure.Messaging;

public static class KafkaFactory
{
    public static IServiceCollection AddKafkaMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var kafkaBroker = configuration["Kafka:Brokers"];
        var completedTransfersTopic = configuration["Kafka:CompletedTransfersTopic"];
        var completedFeeChargesTopic = configuration["Kafka:CompletedFeeChargesTopic"];
        var numberOfPartitions = configuration.GetValue<int>("Kafka:NumberOfPartitions", 3);
        var replicationFactor = configuration.GetValue<short>("Kafka:ReplicationFactor", 1);

        services.AddKafka(kafka => kafka
            .AddCluster(cluster =>
            {
                cluster
                    .WithBrokers(new[] { kafkaBroker })
                    .CreateTopicIfNotExists(
                        topicName: completedTransfersTopic,
                        numberOfPartitions: numberOfPartitions,
                        replicationFactor: replicationFactor)
                    .AddConsumer(consumer =>
                        consumer
                            .Topic(completedTransfersTopic)
                            .WithGroupId("tariffs-consumer-group")
                            .WithBufferSize(100)
                            .WithWorkersCount(3)
                            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                            .AddMiddlewares(middlewares => middlewares
                                .AddDeserializer<JsonCoreDeserializer>()
                                .AddTypedHandlers(handlers =>
                                    handlers.WithHandlerLifetime(InstanceLifetime.Scoped))
                            )
                    )
                    .CreateTopicIfNotExists(
                        topicName: completedFeeChargesTopic,
                        numberOfPartitions: numberOfPartitions,
                        replicationFactor: replicationFactor)
                    .AddProducer(
                        name: "tariffs-producer",
                        producer => producer
                            .DefaultTopic(completedFeeChargesTopic)
                            .AddMiddlewares(middlewares => middlewares
                                .AddSerializer<JsonCoreSerializer>()
                            )
                    );
            })
        );

        return services;
    }
}