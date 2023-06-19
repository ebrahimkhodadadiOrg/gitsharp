using Microsoft.AspNetCore.SignalR;

namespace SignalRClient.SignalR;

public class SignalRHub : Hub<IClient>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SignalRHub> _logger;

    public SignalRHub(IConfiguration configuration, ILogger<SignalRHub> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// OnConnect event
    /// </summary>
    /// <returns></returns>
    public override Task OnConnectedAsync()
    {
        var id = Context.ConnectionId;

        return base.OnConnectedAsync();
    }

    /// <summary>
    /// OnDisconnect event & Log disconnection
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception e)
    {
        var id = Context.ConnectionId;

        //remove connection from cache
        var cache = _HubCacheService.GetAll().FirstOrDefault(x => x.SocketId == id);
        if (cache != null)
            _HubCacheService.Delete(cache.Id);

        // log disconnect
        var Log = new SignalRStatusLog
        {
            Ref = cache.Ref,
            State = 0,
            Description = id,
            Created = DateTime.Now,
        };
        LogRepository.Create(log);
        Save();

        await base.OnDisconnectedAsync(e);
    }

    /// <summary>
    /// 1- save connection in cache
    /// </summary>
    /// <param name="model">controller detail</param>
    /// <returns></returns>
    public async Task Initilize(ClientModel model)
    {
        _logger.LogInformation($"Initilize called from {model.ClientIp} with id {model.ClientID}", model);


        if (!ValidateClient(model))
            return;

        var id = Context.ConnectionId;

        // save in cache
        var hubEntity = new HubEntity
        {
            SocketId = id,
            Ip = model.ClientIp,
            Ref = model.ClientID,
        };
        var entity = _HubCacheService.Add(hubEntity, DateTimeOffset.Now.AddYears(1));

        // log Connect
        var log = new StatusLog
        {
            Ref = hubEntity.Ref,
            State = 1,
            Description = id,
            Created = DateTime.Now,
        };
        LogRepository.Create(log);
        Save();

        // create client token & call client
        var clientTokenModel = new ClientTokenModel { Id = entity.Id, IP = entity.Ip };
        var clientToken = clientTokenModel.Encrypt();
        await Clients.Caller.ParticipantToken(clientToken);
    }
    /// <summary>
    /// validate client model
    /// </summary>
    /// <param name="model">setting</param>
    /// <returns></returns>
    private bool ValidateClient(ClientModel model)
    {
        if (model.ClientID == 0)
            return false;
        if (string.IsNullOrWhiteSpace(model.ClientIp))
            return false;
        if (string.IsNullOrWhiteSpace(model.Digest))
            return false;

        return true;
    }
    public class ClientModel
    {
        public int ClientID { get; set; }
        public string ClientIp { get; set; }
        public DateTime RequestDateTime { get; set; }
    }
}
