using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories;

public class ServiceRepository(IDbConnection connection) : BaseRepository<Service>(connection, "Services"), IServiceRepository
{ }