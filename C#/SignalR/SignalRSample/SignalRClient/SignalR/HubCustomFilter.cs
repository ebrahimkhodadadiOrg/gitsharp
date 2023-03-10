using Microsoft.AspNetCore.SignalR;

namespace SignalRClient.SignalR;

public static class HubCustomFilter
{
    public static void AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            // Global filters will run first
            options.AddFilter<CustomFilter>();
        });

        //Serilog.Log.Information("Starting SignalR");
    }
    public class CustomFilter : IHubFilter
    {
        ILogger<CustomFilter> _logger;
        public CustomFilter(ILogger<CustomFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            _logger.LogInformation($"Calling hub method '{invocationContext.HubMethodName}'");
            try
            {
                return await next(invocationContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception calling '{invocationContext.HubMethodName}': {ex}");

                throw;
            }
        }

        public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            _logger.LogInformation($"Client connected to hub '{context.Context.ConnectionId}'");

            await next(context);
        }

        public async Task OnDisconnectedAsync(
            HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            _logger.LogInformation($"Client disconnected from hub '{context.Context.ConnectionId}'");

            await next(context, exception);
        }
    }
}
