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
    public async Task<ActionResult<ServiceCategoryResponseDto>> CreateServiceCategoryAsync([FromBody] ServiceCategoryCreateRequestDto dto, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.CreateAsync(dto, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceCategoryResponseDto>> UpdateServiceCategoryAsync([FromBody] ServiceCategoryUpdateRequestDto dto, [FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.UpdateAsync(dto, id, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteServiceCategoryAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await serviceCategoryService.DeleteAsync(id, cancellationToken);

        return Ok(ControllerMessages.ServiceCategoryDeletedSuccessfullyMessage);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceCategoryResponseDto?>> GetServiceCategoryByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetByIdAsync(id, cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ServiceCategoryResponseDto>>> GetServiceCategoriesAsync(CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetAllAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("with-dependencies")]
    public async Task<ActionResult<IReadOnlyCollection<ServiceCategoryResponseDto>>> GetServiceCategoriesWithDependenciesAsync(CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetAllWithDependenciesAsync(cancellationToken));

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/with-dependencies")]
    public async Task<ActionResult<ServiceCategoryResponseDto>> GetServiceCategoryByIdWithDependenciesAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await serviceCategoryService.GetWithDependenciesAsync(id, cancellationToken));
}