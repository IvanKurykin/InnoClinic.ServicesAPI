using Application.DTO.Specialization;
using Application.Interfaces;
using Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationController(ISpecializationService specializationService) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<SpecializationResponseDto>> CreateSpecializationAsync([FromBody] SpecializationCreateRequestDto dto, CancellationToken cancellationToken) =>
        Ok(await specializationService.CreateAsync(dto, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<SpecializationResponseDto>> UpdateSpecializationAsync([FromBody] SpecializationUpdateRequestDto dto, [FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await specializationService.UpdateAsync(dto, id, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSpecializationAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await specializationService.DeleteAsync(id, cancellationToken);

        return Ok(ControllerMessages.SpecializationDeletedSuccessfullyMessage);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<SpecializationResponseDto?>> GetSpecializationByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        await specializationService.GetByIdAsync(id, cancellationToken);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SpecializationResponseDto>>> GetSpecializationsAsync(CancellationToken cancellationToken) =>
        Ok(await specializationService.GetAllAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("with-dependencies")]
    public async Task<ActionResult<IReadOnlyCollection<SpecializationResponseDto>>> GetSpecializationsWithDependenciesAsync(CancellationToken cancellationToken) =>
        Ok(await specializationService.GetAllWithDependenciesAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/with-dependencies")]
    public async Task<ActionResult<SpecializationResponseDto>> GetSpecializationByIdWithDependenciesAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await specializationService.GetWithDependenciesAsync(id, cancellationToken));
}