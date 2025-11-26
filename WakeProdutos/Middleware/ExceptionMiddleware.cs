using WakeProdutos.Shared.Constants;
using WakeProdutos.Shared.Results;
using System.Net;
using System.Text.Json;

namespace WakeProdutos.API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message = exception.Message;

            switch (exception)
            {
                case ArgumentException:
                case InvalidOperationException:
                    status = HttpStatusCode.BadRequest; // 400
                    break;

                case KeyNotFoundException:
                    status = HttpStatusCode.NotFound;   // 404
                    break;

                default:
                    status = HttpStatusCode.InternalServerError; // 500
                    message = Constantes.MensagemFalhaPadrao;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = JsonSerializer.Serialize(Result<string>
                .Fail(context.Response.StatusCode, message, null));

            return context.Response.WriteAsync(response);
        }
    }
}
