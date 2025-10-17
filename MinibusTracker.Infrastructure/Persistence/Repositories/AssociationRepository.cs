using Dapper;
using MinibusTracker.Application.Abstractions.Persistence;
using MinibusTracker.Application.Common.Interfaces;
using MinibusTracker.Domain.Entities;
using MinibusTracker.Infrastructure.Dapper;
using MinibusTracker.Infrastructure.Persistence.Common;

namespace MinibusTracker.Infrastructure.Persistence.Repositories;

public class AssociationRepository : IAssociationRepository
{
    private readonly IDBConnectionFactory _connectionFactory;
    private readonly ILoggerManager _logger;

    public AssociationRepository(IDBConnectionFactory connectionFactory, ILoggerManager _logger)
    {
        this._connectionFactory = connectionFactory;
        this._logger = _logger;
        DapperHelper.UsePropertyAttributeMapping<Association>();
    }

    public async Task<Guid> CreateAsync(Association associationEntity, CancellationToken cancellationToken = default)
    {
        try
        {
            Guid id = associationEntity.AssociationId == Guid.Empty ? Guid.NewGuid() : associationEntity.AssociationId;
            const string sql = @"
                    INSERT INTO associations (association_id, name, region, contact_phone)
                    VALUES (@Id, @Name, @Region, @ContactPhone);";

            using var conn = _connectionFactory.create();

            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                associationEntity.Name,
                associationEntity.Region,
                associationEntity.ContactPhone
            });
            _logger.LogInfo($"Association created: {associationEntity.Name} ({id})");
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating association", ex);
            throw 
                new InvalidOperationException($"Failed to create new association with name '{associationEntity.Name}'", ex);
        }
    }

    public async Task<Association?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try {

            const string sql = @"
                SELECT association_id, name, region, contact_phone, created_at, updated_at
                FROM associations
                WHERE association_id = @Id
                LIMIT 1;";

            using var conn = _connectionFactory.create();
            
            Association? association =  
                await conn.QueryFirstOrDefaultAsync<Association>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
            if (association == null)
            {
                _logger.LogWarn($"Association with ID '{id}' not found.");
                throw new KeyNotFoundException($"Association with ID '{id}' not found.");
            }
            return association;

        } catch (Exception ex)
        {
            _logger.LogError("Error retrieving association by ID", ex);
            throw new InvalidOperationException($"Failed to retrieve association with ID '{id}'", ex);
        }
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Association>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            const string sql = @"
                SELECT association_id, name, region, contact_phone, created_at, updated_at
                FROM associations
                ORDER BY name;";

            using var conn = _connectionFactory.create();
            var rows = await conn.QueryAsync<Association>(new CommandDefinition(sql, cancellationToken: cancellationToken));
            Association[] associations = rows.AsList().ToArray();
            return associations;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all associations", ex);
            throw new InvalidOperationException("Failed to retrieve all associations", ex);
        }
    }

    public async Task<bool> UpdateAsync(Association entity, CancellationToken cancellationToken = default)
    {
        try
        {
            const string sql = @"
            UPDATE associations
            SET name = @Name,
                region = @Region,
                contact_phone = @ContactPhone,
                updated_at = CURRENT_TIMESTAMP
            WHERE association_id = @Id;";
            
            using var conn = _connectionFactory.create();
            var affected = await conn.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    Id = entity.AssociationId,
                    entity.Name,
                    entity.Region,
                    entity.ContactPhone
                },
                cancellationToken: cancellationToken));

            return affected > 0;

        }
        catch(Exception ex) { 
            _logger.LogError("Error updating association", ex);
            throw new InvalidOperationException($"Failed to update association with ID '{entity.AssociationId}'", ex);
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {

            const string sql = @"DELETE FROM associations WHERE association_id = @Id;";
            using var conn = _connectionFactory.create();
            var affected = await conn.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return affected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting association", ex);
            throw new InvalidOperationException($"Failed to delete association with ID '{id}'", ex);
        }
    }
}