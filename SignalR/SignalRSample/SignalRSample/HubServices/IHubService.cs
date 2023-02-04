using SignalRServer.Models;
using static SignalRServer.HubServices.HubService;

namespace SignalRServer.HubServices;

public interface IHubService
{
    public bool IsConnected { get; }

    event Action<string> ParticipantToken;

    Task Initilize(ClientModel model);
    Task ConnectAsync();
}
