using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Helpers.Constants;

namespace Infrastructure.Services;

public class BaseService<TEntity, TCreateRequestDto, TUpdateRequestDto, TResponseDto> : IService<TCreateRequestDto, TUpdateRequestDto, TResponseDto> where TEntity : class where TCreateRequestDto : class where TUpdateRequestDto : class where TResponseDto : class
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    protected BaseService(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TResponseDto> CreateAsync(TCreateRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (dto is null) throw new BadRequestException<TEntity>(ErrorMessages.RequestDTOCannotBeNull);

        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.CreateAsync(entity, cancellationToken);

        if (result is null) throw new BadRequestException<TEntity>(ErrorMessages.FailedToCreateEntity);

        return _mapper.Map<TResponseDto>(result);
    }

    public virtual async Task<TResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(id, cancellationToken);

        if (result is null) throw new NotFoundException<TEntity>(id);

        return _mapper.Map<TResponseDto>(result);
    }

    public virtual async Task<IReadOnlyCollection<TResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IReadOnlyCollection<TResponseDto>>(result);
    }

    public virtual async Task<TResponseDto> UpdateAsync(TUpdateRequestDto dto, Guid id, CancellationToken cancellationToken = default)
    {
        if (dto is null) throw new BadRequestException<TEntity>(ErrorMessages.RequestDTOCannotBeNull);

        var entity = await _repository.GetByIdAsync(id, cancellationToken);

        _mapper.Map(dto, entity);

        if (entity is null) throw new NotFoundException<TEntity>(id);

        var result = await _repository.UpdateAsync(entity, cancellationToken);

        return _mapper.Map<TResponseDto>(result);
    }
}