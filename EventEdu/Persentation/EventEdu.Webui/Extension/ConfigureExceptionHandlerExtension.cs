using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using EventEdu.Application.Exceptions; 

namespace EventEdu.Webui.Extension
{
	public static class ConfigureExceptionHandlerExtension
	{
		public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
		{
			application.UseExceptionHandler(builder =>
			{
				builder.Run(async context =>
				{
					context.Response.ContentType = MediaTypeNames.Application.Json;

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						var statusCode = contextFeature.Error switch
						{
							NotFoundException => (int)HttpStatusCode.NotFound,
							BadRequestException => (int)HttpStatusCode.BadRequest,
							UnauthorizedException => (int)HttpStatusCode.Unauthorized,
							ValidationException => (int)HttpStatusCode.BadRequest,
							_ => (int)HttpStatusCode.InternalServerError
						};

						context.Response.StatusCode = statusCode;

						logger.LogError($"Error: {contextFeature.Error.Message}");

						await context.Response.WriteAsync(JsonSerializer.Serialize(new
						{
							StatusCode = statusCode,
							Message = contextFeature.Error.Message,
							Title = "Xeta alindi"
						}));
					}
				});
			});
		}
	}
}