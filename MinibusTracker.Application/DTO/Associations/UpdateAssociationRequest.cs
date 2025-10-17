using MinibusTracker.Application.DTO.Associations.Common;

namespace MinibusTracker.Application.DTO.Associations;

public class UpdateAssociationRequest : BaseAssociation
{
    public Guid AssociationId { get; set; }
}