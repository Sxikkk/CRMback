using System.Diagnostics;
using System.Text.Json;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        var stopwatch = Stopwatch.StartNew();
        
        context.Request.EnableBuffering();
        
        var body = await ReadBodyAsync(context.Request);
        
        _logger.LogInformation("[{RequestId}] → {Method} {Path} started", requestId, context.Request.Method, context.Request.Path);
        _logger.LogInformation("[{RequestId}] Query: {QueryString}", requestId, context.Request.QueryString);
        _logger.LogInformation("[{RequestId}] Headers: {Headers}", requestId, 
            JsonSerializer.Serialize(context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())));
        
        if (!string.IsNullOrEmpty(body))
        {
            _logger.LogInformation("[{RequestId}] Body: {Body}", requestId, body);
        }
        
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        
        try
        {
            await _next(context);
            
            stopwatch.Stop();
            _logger.LogInformation("[{RequestId}] ← {Method} {Path} completed in {ElapsedMs}ms with status {StatusCode}", 
                requestId, context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "[{RequestId}] ✗ {Method} {Path} failed after {ElapsedMs}ms", 
                requestId, context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            
            _logger.LogInformation("[{RequestId}] Response body: {ResponseBody}", requestId, responseBodyText);
            
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
    
    private async Task<string> ReadBodyAsync(HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}