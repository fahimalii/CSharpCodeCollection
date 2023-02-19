using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Primitives;

namespace SerilogConfigurationSample.Middleware;

public class CorelationIdProviderMiddleware
{
    private const string CorelationId = "CorelationId";
    private const string AppRequestTraceId = "AppRequestTraceId";

    private readonly RequestDelegate nextRequestDelegate;
    private readonly ILogger logger;

    public CorelationIdProviderMiddleware(RequestDelegate nextRequestDelegate,
        ILogger<CorelationIdProviderMiddleware> logger)
    {
        this.nextRequestDelegate = nextRequestDelegate ?? throw new ArgumentNullException(nameof(nextRequestDelegate));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext httpContext, CorelationIdProvider corelationIdProvider)
    {
        //if (!httpContext.Request.Headers.TryGetValue(CorelationId, out StringValues corelationId)
        //    || string.IsNullOrWhiteSpace(corelationId))
        //{
        //    corelationId = corelationIdProvider.CorelationId;
        //    httpContext.Request.Headers.Add(CorelationId, corelationId);
        //}

        //httpContext.Response.Headers.Add(CorelationId, corelationId);

        using (logger.BeginScope(new Dictionary<string, object>
        {
            [CorelationId] = corelationIdProvider.CorelationId,
            [AppRequestTraceId] = httpContext.TraceIdentifier
        }))
        {
            await nextRequestDelegate(httpContext);
        }
    }
}
