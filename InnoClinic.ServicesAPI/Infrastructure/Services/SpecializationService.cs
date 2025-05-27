using Application.DTO.Specialization;
using Application.Exceptions.SpecializationExceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using InnoClinic.Messaging.Abstractions;

namespace Infrastructure.Services;

public class SpecializationService(ISpecializationRepository repository, IMapper mapper, IMessagePublisher messagePublisher) : BaseService<Specialization, SpecializationCreateRequestDto, SpecializationUpdateRequestDto, SpecializationResponseDto>(repository, mapper, messagePublisher), ISpecializationService
{
    public async Task<SpecializationResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var specialization = await repository.GetWithDependenciesAsync(id, cancellationToken);

        if (specialization is null) throw new SpecializationNotFoundException(id);

        return _mapper.Map<SpecializationResponseDto?>(specialization);
    }

    public async Task<IReadOnlyCollection<SpecializationResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default)
    {
        var specializations = await repository.GetAllWithDependenciesAsync(cancellationToken);

        return _mapper.Map<IReadOnlyCollection<SpecializationResponseDto>>(specializations);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.GetByIdAsync(id, cancellationToken);

        if (exists is null) throw new SpecializationNotFoundException(id);

        await repository.DeleteAsync(id, cancellationToken);
    }
}