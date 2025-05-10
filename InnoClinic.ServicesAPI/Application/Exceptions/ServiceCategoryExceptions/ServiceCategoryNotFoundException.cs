using System.Diagnostics.CodeAnalysis;
using Domain.Entities;

namespace Application.Exceptions.ServiceCategoryExceptions;

[ExcludeFromCodeCoverage]
public class ServiceCategoryNotFoundException(Guid id) : NotFoundException<ServiceCategory>(id)
{ }