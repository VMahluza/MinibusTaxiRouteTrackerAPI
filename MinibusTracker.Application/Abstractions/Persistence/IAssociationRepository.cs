using MinibusTracker.Domain.Entities;

namespace MinibusTracker.Application.Abstractions.Persistence;
public interface IAssociationRepository
{
    Task<Guid> CreateAsync(Association association, CancellationToken cancellationToken = default);
    Task<Association?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Association>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Association association, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
