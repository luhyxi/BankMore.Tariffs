using BankMore.Tariffs.Domain.Interfaces;
using BankMore.Tariffs.Domain.TariffAggregate;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using TariffModel = BankMore.Tariffs.Domain.TariffAggregate.Tariff;

namespace BankMore.Tariffs.Application.Tariff.Create;

public sealed class CreateTariffHandler : IMessageHandler<CreateTariffCommand>
{
    private readonly ILogger<CreateTariffHandler> _logger;
    private readonly ITariffRepository _tariffRepository;
    private readonly IProducerAccessor _producerAccessor;
    private readonly IConfiguration _configuration;

    public CreateTariffHandler(
        ILogger<CreateTariffHandler> logger,
        ITariffRepository tariffRepository,
        IProducerAccessor producerAccessor,
        IConfiguration configuration)
    {
        _logger = logger;
        _tariffRepository = tariffRepository;
        _producerAccessor = producerAccessor;
        _configuration = configuration;
    }

    public async Task Handle(IMessageContext context, CreateTariffCommand message)
    {
        try
        {
            _logger.LogInformation(
                "Processing transfer fee for RequestId: {RequestId}, AccountId: {AccountId}",
                message.RequestId,
                message.AccountId);
            
            var feeAmount = _configuration.GetValue<decimal>("Tariff:Value");

            var tariff = new TariffModel
            {
                IdContaCorrente = message.AccountId,
                DataMovimento = DateTime.UtcNow,
                Valor = feeAmount
            };

            await _tariffRepository.CreateAsync(tariff);

            _logger.LogInformation(
                "Tariff recorded successfully. AccountId: {AccountId}, Amount: {Amount}",
                message.AccountId,
                feeAmount);

            var producer = _producerAccessor.GetProducer("tariffs-producer");
            var completedFeeMessage = new TariffChargedEvent(
                message.AccountId,
                feeAmount);

            await producer.ProduceAsync(
                _configuration["Kafka:CompletedFeeChargesTopic"],
                null,
                completedFeeMessage);

            _logger.LogInformation(
                "Fee charge completion message sent to Kafka. AccountId: {AccountId}",
                message.AccountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing tariff for RequestId: {RequestId}, AccountId: {AccountId}",
                message.RequestId,
                message.AccountId);
            throw;
        }
    }
}