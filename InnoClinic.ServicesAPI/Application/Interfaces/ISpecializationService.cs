using Application.DTO.Specialization;

namespace Application.Interfaces;

public interface ISpecializationService : IService<SpecializationRequestDto, SpecializationResponseDto>
{
    Task<SpecializationResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<SpecializationResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}