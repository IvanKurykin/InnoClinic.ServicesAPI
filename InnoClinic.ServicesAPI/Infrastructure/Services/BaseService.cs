using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Helpers.Constants;

namespace Infrastructure.Services;

public class BaseService<TEntity, TRequestDto, TResponseDto> : IService<TRequestDto, TResponseDto> where TEntity : class where TRequestDto : class where TResponseDto : class
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    protected BaseService(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TResponseDto> CreateAsync(TRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (dto is null) throw new BadRequestException<TEntity>(ServiceConstants.RequestDTOCannotBeNull);

        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.CreateAsync(entity, cancellationToken);

        if (result is null) throw new BadRequestException<TEntity>(ServiceConstants.FailedToCreateEntity);

        return _mapper.Map<TResponseDto>(result);
    }

    public async Task<TResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(id, cancellationToken);

        if (result is null) throw new NotFoundException<TEntity>(id);

        return _mapper.Map<TResponseDto>(result);
    }

    public async Task<IList<TResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IList<TResponseDto>>(result);
    }

    public async Task<TResponseDto> UpdateAsync(TRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (dto is null) throw new BadRequestException<TEntity>(ServiceConstants.RequestDTOCannotBeNull);

        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.UpdateAsync(entity, cancellationToken);

        if (result is null) throw new BadRequestException<TEntity>(ServiceConstants.FailedToUpdateEntity);

        return _mapper.Map<TResponseDto>(result);
    }
}