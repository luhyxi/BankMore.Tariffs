namespace BankMore.Tariffs.Application.Tariff.Create;

/// <summary>
/// Event published to Kafka when a fee has been successfully charged.
/// This will be consumed by the CheckingAccount API to debit the account.
/// </summary>
public sealed record TariffChargedEvent(
    string AccountId,
    decimal ChargedAmount);