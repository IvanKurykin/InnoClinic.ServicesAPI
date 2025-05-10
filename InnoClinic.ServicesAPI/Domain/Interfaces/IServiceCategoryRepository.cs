using Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceCategoryRepository : IRepository<ServiceCategory>
{
    Task<ServiceCategory?> GetWithServicesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<ServiceCategory>> GetAllWithServicesAsync(CancellationToken cancellationToken = default);
    Task DeleteWithServicesAsync(Guid id, CancellationToken cancellationToken = default);
}