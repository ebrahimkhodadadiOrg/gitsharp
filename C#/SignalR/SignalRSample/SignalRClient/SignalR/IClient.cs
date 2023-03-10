namespace SignalRClient.SignalR;

public interface IClient
{
    /// <summary>
    /// send configuration to client
    /// </summary>
    /// <param name="token"></param>
    Task ParticipantToken(string token);
}