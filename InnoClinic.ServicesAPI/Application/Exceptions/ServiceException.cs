using System.Diagnostics.CodeAnalysis;

namespace Application.Exceptions;

[ExcludeFromCodeCoverage]
public class ServiceException : Exception
{
    protected ServiceException(string message) : base(message) { }
}