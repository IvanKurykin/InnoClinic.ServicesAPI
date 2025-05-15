using System.Text.Json;
using API.Middleware;
using Application.Exceptions;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using UnitTests.TestCases;

namespace UnitTests;

public class ExceptionHandlingMiddlewareTests
{
    private static async Task<string> GetResponseBodyAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        return await new StreamReader(response.Body).ReadToEndAsync();
    }

    [Fact]
    public async Task InvokeAsyncShouldReturn400ForValidationException()
    {
        var httpContext = new DefaultHttpContext();
        await using var memoryStream = new MemoryStream();
        httpContext.Response.Body = memoryStream;

        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new(ExceptionHandlingMiddlewareTestData.NameField, ExceptionHandlingMiddlewareTestData.NameError),
            new(ExceptionHandlingMiddlewareTestData.EmailField, ExceptionHandlingMiddlewareTestData.EmailError)
        };
        var exception = new ValidationException(failures);

        var middleware = new ExceptionHandlingMiddleware(_ => throw exception);

        await middleware.InvokeAsync(httpContext);

        httpContext.Response.StatusCode.Should().Be(ExceptionHandlingMiddlewareTestData.Status400BadRequest);

        var response = await GetResponseBodyAsync(httpContext.Response);
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.ErrorProperty).GetString().Should().Be(ExceptionHandlingMiddlewareTestData.ValidationFailedMessage);

        var details = json.GetProperty(ExceptionHandlingMiddlewareTestData.DetailsProperty).EnumerateArray().ToList();
        details.Should().HaveCount(2);
    }

    [Fact]
    public async Task InvokeAsyncShouldReturn404ForNotFoundExceptionOfService()
    {
        var httpContext = new DefaultHttpContext();
        await using var memoryStream = new MemoryStream();
        httpContext.Response.Body = memoryStream;

        var exception = new NotFoundException<Service>(ExceptionHandlingMiddlewareTestData.NotFoundServiceMessage);

        var middleware = new ExceptionHandlingMiddleware(_ => throw exception);

        await middleware.InvokeAsync(httpContext);

        httpContext.Response.StatusCode.Should().Be(ExceptionHandlingMiddlewareTestData.Status404NotFound);

        var response = await GetResponseBodyAsync(httpContext.Response);
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.ErrorProperty).GetString().Should().Be(ExceptionHandlingMiddlewareTestData.NotFoundServiceMessage);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.DetailsProperty).ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task InvokeAsyncShouldReturn400ForBadRequestExceptionOfSpecialization()
    {
        var httpContext = new DefaultHttpContext();
        await using var memoryStream = new MemoryStream();
        httpContext.Response.Body = memoryStream;

        var exception = new BadRequestException<Specialization>(
            ExceptionHandlingMiddlewareTestData.BadRequestSpecializationMessage);

        var middleware = new ExceptionHandlingMiddleware(_ => throw exception);

        await middleware.InvokeAsync(httpContext);

        httpContext.Response.StatusCode.Should().Be(ExceptionHandlingMiddlewareTestData.Status400BadRequest);

        var response = await GetResponseBodyAsync(httpContext.Response);
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.ErrorProperty).GetString().Should().Contain(ExceptionHandlingMiddlewareTestData.BadRequestSpecializationMessage);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.DetailsProperty).ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task InvokeAsyncShouldReturn500ForUnhandledException()
    {
        var httpContext = new DefaultHttpContext();
        await using var memoryStream = new MemoryStream();
        httpContext.Response.Body = memoryStream;

        var exception = new Exception(ExceptionHandlingMiddlewareTestData.UnhandledErrorMessage);

        var middleware = new ExceptionHandlingMiddleware(_ => throw exception);

        await middleware.InvokeAsync(httpContext);

        httpContext.Response.StatusCode.Should().Be(ExceptionHandlingMiddlewareTestData.Status500InternalServerError);

        var response = await GetResponseBodyAsync(httpContext.Response);
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.ErrorProperty).GetString().Should().Be(ExceptionHandlingMiddlewareTestData.UnhandledErrorMessage);

        json.GetProperty(ExceptionHandlingMiddlewareTestData.DetailsProperty).ValueKind.Should().Be(JsonValueKind.Null);
    }
}