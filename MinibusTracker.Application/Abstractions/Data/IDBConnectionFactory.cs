using System.Data;

//namespace MinibusTracker.Infrastructure.Persistence.Common;

namespace MinibusTracker.Application.Abstractions.Data;

public interface IDBConnectionFactory
{
    IDbConnection create();
}
