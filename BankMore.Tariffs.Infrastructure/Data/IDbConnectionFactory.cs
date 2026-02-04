using System.Data.Common;

namespace BankMore.Tariffs.Infrastructure.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}