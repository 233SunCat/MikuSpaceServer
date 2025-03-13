using System.Diagnostics;

namespace MikuSpaceServer.Logger
{
    public class ControllerLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ControllerLoggingMiddleware> _logger;

        public ControllerLoggingMiddleware(RequestDelegate next, ILogger<ControllerLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 控制器调用前日志
                LogRequest(context);

                await _next(context); // 执行控制器方法

                // 控制器调用后日志
                LogResponse(context, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LogException(context, ex, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        private void LogRequest(HttpContext context)
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}"
                + $" | ContentType: {context.Request.ContentType}"
                + $" | RemoteIP: {context.Connection.RemoteIpAddress}");
        }

        private void LogResponse(HttpContext context, long durationMs)
        {
            _logger.LogInformation($"Response: {context.Response.StatusCode} | Duration: {durationMs}ms");
        }

        private void LogException(HttpContext context, Exception ex, long durationMs)
        {
            
            _logger.LogError(ex, $"Request failed: {context.Request.Path} | Duration: {durationMs}ms");
        }
    }

}
