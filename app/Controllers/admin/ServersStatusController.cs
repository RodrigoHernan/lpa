using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ServerStatusClient;
using app.Models;

// Microservicios

namespace app.Controllers;

public class ServersStatusController : Controller
{
    private readonly ILogger<ServersStatusController> _logger;

    public ServersStatusController(ILogger<ServersStatusController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var httpHandler = new HttpClientHandler();
        // Return `true` to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        // The port number must match the port of the gRPC server.
        var channel = GrpcChannel.ForAddress("https://localhost:7156",
            new GrpcChannelOptions { HttpHandler = httpHandler });
        var client = new ServerStatus.ServerStatusClient(channel);
        var reply = await client.GetStatusAsync(
                        new GetStatusRequest { Name = Request.Host.Value });
        return View(new ServerStatusViewModel() {Reply=reply});
    }
}
