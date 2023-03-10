namespace SignalRServer.Models;

public class ClientModel
{
    public int ClientID { get; set; }
    public string ClientIp { get; set; }
    public DateTime RequestDateTime { get; set; }
}