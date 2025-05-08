using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class SpecializationRepository(IDbConnection connection) : BaseRepository<Specialization>(connection, "Specializations"), ISpecializationRepository
{ }