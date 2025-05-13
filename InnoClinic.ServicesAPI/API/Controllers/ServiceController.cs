using Application.DTO.Service;
using Application.Interfaces;
using Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController(IServiceService serviceService) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<ServiceResponseDto>> CreateServiceAsync([FromBody] ServiceCreateRequestDto dto, CancellationToken cancellationToken) =>
       Ok(await serviceService.CreateAsync(dto, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponseDto>> UpdateServiceAsync([FromBody] ServiceUpdateRequestDto dto, [FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceService.UpdateAsync(dto, id, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteServiceAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await serviceService.DeleteAsync(id, cancellationToken);

        return Ok(ControllerMessages.ServiceDeletedSuccessfullyMessage);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponseDto?>> GetServiceByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceService.GetByIdAsync(id, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ServiceResponseDto>>> GetServicesAsync(CancellationToken cancellationToken) =>
        Ok(await serviceService.GetAllAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("with-dependencies")]
    public async Task<ActionResult<IReadOnlyCollection<ServiceResponseDto>>> GetServicesWithDependenciesAsync(CancellationToken cancellationToken) =>
        Ok(await serviceService.GetAllWithDependenciesAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/with-dependencies")]
    public async Task<ActionResult<ServiceResponseDto>> GetServiceByIdWithDependenciesAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceService.GetWithDependenciesAsync(id, cancellationToken));
}