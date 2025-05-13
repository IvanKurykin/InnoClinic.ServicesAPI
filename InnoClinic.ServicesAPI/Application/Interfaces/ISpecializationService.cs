using Application.DTO.Specialization;

namespace Application.Interfaces;

public interface ISpecializationService : IService<SpecializationCreateRequestDto, SpecializationUpdateRequestDto, SpecializationResponseDto>
{
    Task<SpecializationResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SpecializationResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}