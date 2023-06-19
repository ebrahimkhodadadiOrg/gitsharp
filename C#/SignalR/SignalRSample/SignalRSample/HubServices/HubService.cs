using Microsoft.AspNetCore.SignalR.Client;
using SignalRServer.Models;

namespace SignalRServer.HubServices;

public class HubService : IHubService
{
    public event Action<string> ParticipantToken;

    public HubConnection connection;
    private const string url = "https://localhost:5001/SignalRHub";

    private readonly ILogger<HubService> _logger;
    public HubService(ILogger<HubService> logger)
    {
        _logger = logger;
    }

    // status hub
    public bool IsConnected => connection.State == HubConnectionState.Connected;

    public async Task ConnectAsync()
    {
        try
        {
            // create connection
            connection = new HubConnectionBuilder()
               .ConfigureLogging(logging =>
               {
                   logging.SetMinimumLevel(LogLevel.Information);
                   logging.AddConsole();
               })
               .WithUrl(url)
               .WithAutomaticReconnect()
               .Build();

            // get token Event token
            connection.On<string>(nameof(ParticipantToken), (token) => { ParticipantToken?.Invoke(token); });

            connection.Reconnecting += Reconnecting;
            connection.Reconnected += Reconnected;
            connection.Closed += Disconnected;

            // start connecting
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Connecting to SignalR");
        }
    }

    private Task Reconnected(string? arg)
    {
        _logger.LogWarning(arg, "Reconnected to SignalR");

        return Task.CompletedTask;
    }

    private Task Reconnecting(Exception? arg)
    {
        _logger.LogWarning(arg, "Reconnecting to SignalR");

        return Task.CompletedTask;
    }

    private Task Disconnected(Exception? arg)
    {
        _logger.LogWarning(arg, "Disconnected from SignalR");

        return Task.CompletedTask;
    }

    // call initilize method server
    public async Task Initilize(ClientModel model)
    {
        await connection.InvokeAsync(nameof(Initilize), model).ConfigureAwait(false);
    }

}