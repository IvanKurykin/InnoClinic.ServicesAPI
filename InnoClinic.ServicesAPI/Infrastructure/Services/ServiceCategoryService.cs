using Application.DTO.ServiceCategory;
using Application.Exceptions.ServiceCategoryExceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using InnoClinic.Messaging.Abstractions;

namespace Infrastructure.Services;

public class ServiceCategoryService(IServiceCategoryRepository repository, IMapper mapper, IMessagePublisher messagePublisher) : BaseService<ServiceCategory, ServiceCategoryCreateRequestDto, ServiceCategoryUpdateRequestDto, ServiceCategoryResponseDto>(repository, mapper, messagePublisher), IServiceCategoryService
{
    public async Task<ServiceCategoryResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var serviceCategory = await repository.GetWithDependenciesAsync(id, cancellationToken);

        if (serviceCategory is null) throw new ServiceCategoryNotFoundException(id);

        return _mapper.Map<ServiceCategoryResponseDto?>(serviceCategory);
    }

    public async Task<IReadOnlyCollection<ServiceCategoryResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default)
    {
        var serviceCategories = await repository.GetAllWithDependenciesAsync(cancellationToken);

        return _mapper.Map<IReadOnlyCollection<ServiceCategoryResponseDto>>(serviceCategories);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await repository.GetByIdAsync(id, cancellationToken);

        if (exists is null) throw new ServiceCategoryNotFoundException(id);

        await repository.DeleteAsync(id, cancellationToken);
    }
}