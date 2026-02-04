using BankMore.Tariffs.Domain.Interfaces;
using BankMore.Tariffs.Domain.TariffAggregate;
using BankMore.Tariffs.Infrastructure.Data;
using Dapper;

namespace BankMore.Tariffs.Infrastructure.Repositories;

public class TariffRepository : ITariffRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    private const string InsertSql =
        """
                INSERT INTO Tariffs (IdContaCorrente, DataMovimento, Valor)
                VALUES (@IdContaCorrente, @DataMovimento, @Valor)";
        """;

    public TariffRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async ValueTask CreateAsync(Tariff tariff, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(InsertSql, tariff);
    }
}