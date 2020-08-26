using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace BlazorHub.Client.Business
{
    public interface IGenericHubClientFactory
    {
        IGenericHubClient Create(Uri hub);
    }

    internal class GenericHubClientFactory : IGenericHubClientFactory
    {
        private readonly IHubConnectionBuilder _connectionBuilder;

        public GenericHubClientFactory(IHubConnectionBuilder connectionBuilder)
        {
            _connectionBuilder = connectionBuilder;
        }

        public IGenericHubClient Create(Uri hub)
        {
            return new GenericHubClient(hub, _connectionBuilder);
        }
    }
}
