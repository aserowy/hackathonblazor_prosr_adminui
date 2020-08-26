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
        public IGenericHubClient Create(Uri hub)
        {
            return new GenericHubClient(hub, new HubConnectionBuilder());
        }
    }
}
