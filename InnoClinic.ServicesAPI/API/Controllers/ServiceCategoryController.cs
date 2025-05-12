using Application.DTO.ServiceCategory;
using Application.Interfaces;
using Infrastructure.Helpers.Constants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceCategoryController(IServiceCategoryService serviceCategoryService) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<ServiceCategoryResponseDto>> CreateSpecializationAsync([FromBody] ServiceCategoryCreateRequestDto dto, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.CreateAsync(dto, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceCategoryResponseDto>> UpdateSpecializationAsync([FromBody] ServiceCategoryUpdateRequestDto dto, [FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.UpdateAsync(dto, id, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSpecializationAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await serviceCategoryService.DeleteAsync(id, cancellationToken);

        return Ok(ControllerMessages.ServiceCategoryDeletedSuccessfullyMessage);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceCategoryResponseDto?>> GetSpecializationByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        await serviceCategoryService.GetByIdAsync(id, cancellationToken);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ServiceCategoryResponseDto>>> GetSpecializationsAsync(CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetAllAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("with-dependencies")]
    public async Task<ActionResult<IReadOnlyCollection<ServiceCategoryResponseDto>>> GetSpecializationsWithDependenciesAsync(CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetAllWithDependenciesAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/with-dependencies")]
    public async Task<ActionResult<ServiceCategoryResponseDto>> GetSpecializationByIdWithDependenciesAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetWithDependenciesAsync(id, cancellationToken));
}