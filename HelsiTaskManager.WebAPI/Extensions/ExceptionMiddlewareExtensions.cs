using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace HelsiTaskManager.WebAPI
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseHelsiExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var logger = app.ApplicationServices.GetService<ILogger>();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is ForbiddenException)
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        }
                        logger?.LogError($"{context.Response.ToJson()}");
                        await context.Response.WriteAsync(new BaseResponse()
                        {
                            IsSuccess = false,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
