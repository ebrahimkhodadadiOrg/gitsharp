using Microsoft.AspNetCore.Mvc;
using SignalRServer.HubServices;
using SignalRServer.Models;

namespace SignalRServer.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubService _HubService;

    public HomeController(ILogger<HomeController> logger, IHubService hubService)
    {
        _logger = logger;
        _HubService = hubService;

        _HubService.ParticipantToken += (token) =>
        {
            _logger.LogInformation($"ParticipantToken: {token}");
        };
    }

    [HttpGet]
    public async Task Index()
    {
        try
        {
            _logger.LogInformation("Start Connecting to SignalR...");

            await _HubService.ConnectAsync();

            if (_HubService.IsConnected)
                _logger.LogInformation("Connected to SignalR");
            else
            {
                _logger.LogCritical("Can't Connect to SignalR");
                return;
            }

            // send initilize model to server
            var model = new ClientModel
            {
                ClientID = 1,
                ClientIp = "192.168.1.2",
                RequestDateTime = DateTime.Now
            };
            await _HubService.Initilize(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Connecting to SignalR");
        }
    }
}
