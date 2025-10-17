using MinibusTracker.Application.DTO.Associations;

namespace MinibusTracker.Application.UseCases.Associations;

public interface IAssociationService 
{ 
    Task<Guid> Create(CreateAssociationRequest createAssociationRequest, CancellationToken cancellationToken = default);
    // TODO: Add other association-related business logic methods here
}
