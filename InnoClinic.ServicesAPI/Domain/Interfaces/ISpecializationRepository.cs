using Domain.Entities;

namespace Domain.Interfaces;

public interface ISpecializationRepository
{
    Task<Specialization?> GetWithServicesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<Specialization>> GetAllWithServicesAsync(CancellationToken cancellationToken = default);
    Task DeleteWithServicesAsync(Guid id, CancellationToken cancellationToken = default);
}