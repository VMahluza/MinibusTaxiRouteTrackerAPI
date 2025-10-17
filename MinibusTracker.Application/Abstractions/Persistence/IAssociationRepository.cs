using MinibusTracker.Domain.Entities;

namespace MinibusTracker.Application.Abstractions.Persistence;
public interface IAssociationRepository
{
    Task<Guid> CreateAsync(Association entity, CancellationToken ct = default);
    Task<Association?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Association>> GetAllAsync(CancellationToken ct = default);
    Task<bool> UpdateAsync(Association entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
