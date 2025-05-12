using Application.DTO.Service;

namespace Application.Interfaces;

public interface IServiceService : IService<ServiceCreateRequestDto, ServiceUpdateRequestDto, ServiceResponseDto>
{
    Task<ServiceResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}