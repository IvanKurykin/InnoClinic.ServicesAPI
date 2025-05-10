namespace Application.Interfaces;

public interface IService<TRequestDto, TResponseDto> where TRequestDto : class where TResponseDto : class
{
    Task<TResponseDto> CreateAsync(TRequestDto dto, CancellationToken cancellationToken = default);
    Task<TResponseDto> UpdateAsync(TRequestDto dto, CancellationToken cancellationToken = default);
    Task<IList<TResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}