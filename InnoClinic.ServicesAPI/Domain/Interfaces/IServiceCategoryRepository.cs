using Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceCategoryRepository : IRepository<ServiceCategory>
{
    Task<ServiceCategory?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<ServiceCategory>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}