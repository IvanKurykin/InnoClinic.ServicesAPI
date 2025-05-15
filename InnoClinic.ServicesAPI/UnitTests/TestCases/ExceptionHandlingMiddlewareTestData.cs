using Microsoft.AspNetCore.Http;

namespace UnitTests.TestCases;

public static class ExceptionHandlingMiddlewareTestData
{
    public const string ValidationFailedMessage = "Validation failed";
    public const string NotFoundServiceMessage = "Service not found";
    public const string BadRequestSpecializationMessage = "Invalid Specialization data";
    public const string UnhandledErrorMessage = "Unexpected error";

    public const string NameField = "Name";
    public const string NameError = "Name is required";
    public const string EmailField = "Email";
    public const string EmailError = "Invalid email";

    public const string ErrorProperty = "error";
    public const string DetailsProperty = "details";

    public const int Status400BadRequest = StatusCodes.Status400BadRequest;
    public const int Status404NotFound = StatusCodes.Status404NotFound;
    public const int Status500InternalServerError = StatusCodes.Status500InternalServerError;
}