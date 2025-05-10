using Application.DTO.Service;

namespace Application.Interfaces;

public interface IServiceService : IService<ServiceRequestDto, ServiceResponseDto>
{
    Task<ServiceResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<ServiceResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}