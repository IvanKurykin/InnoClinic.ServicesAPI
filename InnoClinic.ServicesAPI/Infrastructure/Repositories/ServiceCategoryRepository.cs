using System.Data;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class ServiceCategoryRepository(IDbConnection connection) : BaseRepository<ServiceCategory>(connection, "ServiceCategories"), IServiceCategoryRepository
{ }