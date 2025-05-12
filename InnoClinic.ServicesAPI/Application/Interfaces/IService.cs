namespace Application.Interfaces;

public interface IService<TCreateRequestDto, TUpdateRequestDto, TResponseDto> where TCreateRequestDto : class where TUpdateRequestDto : class  where TResponseDto : class
{
    Task<TResponseDto> CreateAsync(TCreateRequestDto dto, CancellationToken cancellationToken = default);
    Task<TResponseDto> UpdateAsync(TUpdateRequestDto dto, Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}