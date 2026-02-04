namespace BankMore.Tariffs.Application.Tariff.Create;

/// <summary>
/// Message consumed from Kafka topic of completed transfers.
/// Represents a transfer that was successfully completed and now requires fee charging.
/// </summary>
public sealed record CreateTariffCommand(
    string RequestId,
    string AccountId);