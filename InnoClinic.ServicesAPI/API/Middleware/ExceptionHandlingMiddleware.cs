﻿using System.Text.Json;
using Application.Exceptions;
using Domain.Entities;
using FluentValidation;
using Serilog;

namespace API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            Log.Error(e, "An unhandled exception occurred: {ExceptionMessage}", e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "Validation failed",
                details = validationException.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
            }));
            return;
        }

        context.Response.StatusCode = exception switch
        {
            NotFoundException<Service> or NotFoundException<ServiceCategory> or NotFoundException<Specialization> => StatusCodes.Status404NotFound,
            BadRequestException<Service> or BadRequestException<ServiceCategory> or BadRequestException<Specialization> => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = exception.Message,
            details = (object?)null
        }));
    }
}