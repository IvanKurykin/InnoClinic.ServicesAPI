using Application.DTO.ServiceCategory;

namespace Application.Interfaces;

public interface IServiceCategoryService : IService<ServiceCategoryCreateRequestDto, ServiceCategoryUpdateRequestDto, ServiceCategoryResponseDto>
{
    Task<ServiceCategoryResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceCategoryResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}