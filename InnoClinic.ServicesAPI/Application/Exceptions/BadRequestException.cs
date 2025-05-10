using System.Diagnostics.CodeAnalysis;

namespace Application.Exceptions;

[ExcludeFromCodeCoverage]
public class BadRequestException<T>(string message) : ServiceException($"Invalid {typeof(T).Name} data: {message}.")
{ }