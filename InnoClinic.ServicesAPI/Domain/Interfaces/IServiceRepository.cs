using Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceRepository : IRepository<Service>
{
    Task<Service?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Service>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}