using System.Data;

namespace MinibusTracker.Infrastructure.Persistence.Common;

public interface IDBConnectionFactory
{
    IDbConnection create();
}
