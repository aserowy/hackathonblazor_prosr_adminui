using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;

namespace BlazorHub.Client.Business
{
    public interface IGenericHubClientFactory
    {
        IGenericHubClient Create(Uri hub);
    }

    internal class GenericHubClientFactory : IGenericHubClientFactory
    {
        private readonly ILogger<GenericHubClient> _logger;

        public GenericHubClientFactory(ILogger<GenericHubClient> logger)
        {
            _logger = logger;
        }

        public IGenericHubClient Create(Uri hub)
        {
            return new GenericHubClient(hub, new HubConnectionBuilder(), _logger);
        }
    }
}
