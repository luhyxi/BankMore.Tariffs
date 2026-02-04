namespace BankMore.Tariffs.Domain.TariffAggregate;

public sealed class Tariff
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public DateTime DataMovimento { get; set; }
    public decimal Valor { get; set; }
}