using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectivityChecker.Contracts;
using ConnectivityChecker.Contracts.Providers;
using ConnectivityChecker.Core;
using ConnectivityChecker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ConnectivityChecker.Controllers
{
    [Produces("application/json")]
    [Route("api/connections")]
    public class ConnectionsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionsService _connectionsService;

        public ConnectionsController(IConfiguration configuration, ConnectionsService connectionsService)
        {
            _configuration = configuration;
            _connectionsService = connectionsService;
        }
        // GET: api/Connections
        [HttpGet]
        [Produces(typeof(IEnumerable<ConnectionInfo>))]
        public IEnumerable<ConnectionInfo> Get()
        {
            var settings = _configuration.AsEnumerable();
            var connections = _connectionsService.FindConnectionsStringForAzureWebApps(settings);
            return connections;
        }

        [HttpGet("checkall")]
        [Produces(typeof(IEnumerable<ConnectionStatus>))]
        public async Task<IEnumerable<ConnectionStatus>> CheckAll()
        {
            var settings = _configuration.AsEnumerable();
            var connections = _connectionsService.FindConnectionsStringForAzureWebApps(settings);
            return await _connectionsService.CheckConnectionStatus(connections);
        }

        [HttpGet("check/{name}/{provider}")]
        [Produces(typeof(ConnectionStatus))]
        public async Task<IActionResult> Check(string name, ProviderNames provider)
        {
            var setting = _configuration[name];
            if (string.IsNullOrEmpty(setting))
            {
                setting = _configuration.GetConnectionString(name);
            }

            if (!string.IsNullOrEmpty(setting))
            {
                var info = new ConnectionInfo
                {
                    Name = name,
                    Value = setting,
                    Provider = provider
                };

                return Ok(await _connectionsService.CheckConnectionStatus(info));
            }

            return NotFound();
        }

        [HttpPost("check/{provider}")]
        [Produces(typeof(ConnectionStatus))]
        public async Task<IActionResult> CheckConnection(ProviderNames provider, [FromBody] ConnectionString connection)
        {
            var info = new ConnectionInfo
            {
                Value = connection.Value,
                Provider = provider
            };

            return Ok(await _connectionsService.CheckConnectionStatus(info));
        }

    }
}
