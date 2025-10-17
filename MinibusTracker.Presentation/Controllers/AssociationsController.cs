using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinibusTracker.Application.Abstractions.Persistence;
using MinibusTracker.Application.Common.Interfaces;
using MinibusTracker.Application.DTO.Associations;
using MinibusTracker.Application.UseCases.Associations;
using MinibusTracker.Domain.Entities;

namespace MinibusTracker.Presentation.Controllers;


[ApiController]
[Route("api/associations")]
[ApiVersion("1.0")]
public class AssociationsController : ControllerBase
{
    private readonly IAssociationRepository _associationRepository;
    private readonly ILoggerManager _loggerManager;
    private readonly IAssociationService _associationService;

    public AssociationsController(IAssociationRepository associationRepository, ILoggerManager _loggerManager, IAssociationService _associationService)
    {
        this._loggerManager = _loggerManager;
        this._associationRepository = associationRepository;
        this._associationService = _associationService;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssociationRequest createAssociationRequest,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(createAssociationRequest.Name))
            return BadRequest("Association name is required.");

        try
        {
            Guid newAssociationId = await _associationService.Create(createAssociationRequest, cancellationToken);

            // TODO: Introduce getAssociationById method in the service layer
            var createdAssociation = await _associationRepository.GetByIdAsync(newAssociationId, cancellationToken);

            if (createdAssociation == null) 
            {
                _loggerManager.LogError($"Failed to retrieve created association with ID: {newAssociationId}");
                return BadRequest("Failed to retrieve the created association. Please try again.");
            }

            _loggerManager.LogInfo($"Successfully created association with ID: {newAssociationId}");
            
            return CreatedAtAction(
                nameof(GetById),           // Action name
                new { id = createdAssociation.AssociationId },  // Route values (only 'id')
                createdAssociation              // Response body
                );
        }
        catch (InvalidOperationException ex)
        {
            _loggerManager.LogError($"Invalid operation while creating association: {createAssociationRequest.Name}", ex);
            return BadRequest("Invalid operation. Please check your request data.");
        }
        catch (ArgumentException ex)
        {
            _loggerManager.LogError($"Invalid argument while creating association: {createAssociationRequest.Name}", ex);
            return BadRequest("Invalid data provided.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"Unexpected error while creating association: {createAssociationRequest.Name}", ex);
            return StatusCode(500, "An unexpected error occurred while creating the association.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var association = await _associationRepository.GetByIdAsync(id, cancellationToken);
            if (association == null)
            {
                _loggerManager.LogInfo($"Association with ID: {id} not found");
                return NotFound();
            }
            
            _loggerManager.LogInfo($"Successfully retrieved association with ID: {id}");
            return Ok(association);
        }
        catch (InvalidOperationException ex)
        {
            _loggerManager.LogError($"Invalid operation while retrieving association with ID: {id}", ex);
            return StatusCode(500, "An invalid operation occurred while retrieving the association.");
        }
        catch (ArgumentException ex)
        {
            _loggerManager.LogError($"Invalid argument while retrieving association with ID: {id}", ex);
            return BadRequest("Invalid association ID provided.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"Unexpected error while retrieving association with ID: {id}", ex);
            return StatusCode(500, "An unexpected error occurred while retrieving the association.");
        }
    }

    // GET: api/associations
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var rows = await _associationRepository.GetAllAsync(cancellationToken);
        return Ok(rows);
    }

    // PUT: api/associations/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id, 
        [FromBody] UpdateAssociationRequest request, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty || request.AssociationId == Guid.Empty || id != request.AssociationId)
            return BadRequest("Mismatched or empty id.");

        var existing = await _associationRepository
            .GetByIdAsync(id, cancellationToken);
        if (existing is null) 
            return NotFound();

        if (!string.IsNullOrWhiteSpace(request.Name)) 
            existing.Name = request.Name.Trim();

        if (request.Region != null) 
            existing.Region = string.IsNullOrWhiteSpace(request.Region) ? null : request.Region.Trim();
        
        if (request.ContactPhone != null) 
            existing.ContactPhone = string.IsNullOrWhiteSpace(request.ContactPhone) ? null : request.ContactPhone.Trim();

        var ok = await _associationRepository
            .UpdateAsync(existing, cancellationToken);

        if (!ok) 
            return Problem("Update failed.");
        var refreshed = await _associationRepository.GetByIdAsync(id, cancellationToken);
        return Ok(refreshed);
    }

    // DELETE: api/associations/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var existing = await _associationRepository.GetByIdAsync(id, ct);
            if (existing is null) 
                return NotFound();

            var ok = await _associationRepository.DeleteAsync(id, ct);
            
            if (!ok)
            {
                _loggerManager.LogError($"Failed to delete association with ID: {id}");
                return Problem("Delete failed.");
            }

            _loggerManager.LogInfo($"Successfully deleted association with ID: {id}");
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _loggerManager.LogError($"Invalid operation while deleting association with ID: {id}", ex);
            return StatusCode(500, "An invalid operation occurred while deleting the association.");
        }
        catch (ArgumentException ex)
        {
            _loggerManager.LogError($"Invalid argument while deleting association with ID: {id}", ex);
            return BadRequest("Invalid association ID provided.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"Unexpected error while deleting association with ID: {id}", ex);
            return StatusCode(500, "An unexpected error occurred while deleting the association.");
        }
    }
}