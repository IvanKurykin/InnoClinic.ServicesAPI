using System.Diagnostics.CodeAnalysis;
using Application.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException<T> : ServiceException
{
    public NotFoundException(Guid id) : base($"{typeof(T).Name} with id {id} was not found.") { }

    public NotFoundException(string message) : base(message) { }
}