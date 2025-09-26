using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text.Json;
using Shared.API.ErrorHandling;
using Shared.Exceptions;
using Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Shared.API.ErrorHandling
{
    public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        private readonly ILogger _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during the request");

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                BaseException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorResponse = new ErrorResponse
            {
                TraceId = httpContext.TraceIdentifier,
                StatusCode = httpContext.Response.StatusCode,
                Message = ex.InnerException?.Message ?? ex.Message,
                Method = httpContext.Request.Method,
                Path = httpContext.Request.GetEncodedPathAndQuery(),
                ErrorCode = ex is BaseException baseEx ? baseEx.ErrorCode : 0,
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
public static class ExceptionMiddlewareExtensions
{
    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}