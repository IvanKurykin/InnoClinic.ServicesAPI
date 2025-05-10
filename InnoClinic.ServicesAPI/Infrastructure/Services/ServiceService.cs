using Application.DTO.Service;
using Application.Exceptions.ServiceExceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class ServiceService(IServiceRepository repository, IMapper mapper) : BaseService<Service, ServiceRequestDto, ServiceResponseDto>(repository, mapper), IServiceService
{
    public async Task<ServiceResponseDto?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = await repository.GetWithDependenciesAsync(id, cancellationToken);

        if (service is null) throw new ServiceNotFoundException(id);

        return _mapper.Map<ServiceResponseDto>(service);
    }

    public async Task<IList<ServiceResponseDto>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default)
    {
        var services = await repository.GetAllWithDependenciesAsync(cancellationToken);

        return _mapper.Map<IList<ServiceResponseDto>>(services);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.GetByIdAsync(id, cancellationToken);

        if (exists is null) throw new ServiceNotFoundException(id);

        await repository.DeleteAsync(id, cancellationToken);
    }
}