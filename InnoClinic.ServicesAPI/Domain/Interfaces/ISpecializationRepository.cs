using Domain.Entities;

namespace Domain.Interfaces;

public interface ISpecializationRepository : IRepository<Specialization>
{
    Task<Specialization?> GetWithDependenciesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Specialization>> GetAllWithDependenciesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}