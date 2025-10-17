using MinibusTracker.Application.Abstractions.Persistence;
using MinibusTracker.Application.DTO.Associations;
using MinibusTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinibusTracker.Application.UseCases.Associations;
public class AssociationService : IAssociationService
{
    private readonly IAssociationRepository _associationRepository;
    public AssociationService(IAssociationRepository associationRepository)
    {
        _associationRepository = associationRepository;
    }

    public async Task<Guid> Create(CreateAssociationRequest createAssociationRequest, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(createAssociationRequest.Name))
            throw new ArgumentException("Name is required.");

        var entity = new Association
        {
            AssociationId = Guid.NewGuid(),
            Name = createAssociationRequest.Name.Trim(),
            Region = createAssociationRequest.Region,
            ContactPhone = createAssociationRequest.ContactPhone
        };

        return await _associationRepository.CreateAsync(entity, cancellationToken);
    }
}
