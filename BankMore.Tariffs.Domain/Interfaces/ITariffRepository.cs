using BankMore.Tariffs.Domain.TariffAggregate;

namespace BankMore.Tariffs.Domain.Interfaces;

public interface ITariffRepository
{
    ValueTask CreateAsync(Tariff tariff, CancellationToken cancellationToken = default);
}