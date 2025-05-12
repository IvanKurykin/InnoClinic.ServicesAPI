using System.Diagnostics.CodeAnalysis;
using Domain.Entities;

namespace Application.Exceptions.ServiceExceptions;

[ExcludeFromCodeCoverage]
public class ServiceNotFoundException(Guid id) : NotFoundException<Service>(id)
{ }