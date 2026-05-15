using System.Net;
using System.Text.Json;
using Mec126.Common.Exceptions;
using Mec126.Common.Responses;

namespace Mec126.Common.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger,
			IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

			var (statusCode, message) = ex switch
			{
				NotFoundException => (HttpStatusCode.NotFound, ex.Message),
				ConflictException => (HttpStatusCode.Conflict, ex.Message),
				ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
				_ => (HttpStatusCode.InternalServerError,
					_env.IsDevelopment() ? ex.Message : "An unexpected error occurred.")
			};

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			var response = ApiResponse.Fail(message);

			await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
		}

		private static readonly JsonSerializerOptions JsonOptions = new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};
	}
}
