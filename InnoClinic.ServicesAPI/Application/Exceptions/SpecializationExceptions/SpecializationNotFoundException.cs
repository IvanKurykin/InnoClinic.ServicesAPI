using System.Diagnostics.CodeAnalysis;
using Domain.Entities;

namespace Application.Exceptions.SpecializationExceptions;

[ExcludeFromCodeCoverage]
public class SpecializationNotFoundException(Guid id) : NotFoundException<Specialization>(id)
{ }