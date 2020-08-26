using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHub.Client.Business
{
    public interface IGenericHubClient : IDisposable
    {
        Task InitializeHubConnection();

        void RegisterBinding(string calls, Action<string> handleResponse);
        Task InvokeAsync(string action, string model);
    }

    internal class GenericHubClient : IGenericHubClient
    {
        private readonly Uri _hub;
        private readonly IHubConnectionBuilder _hubConnectionBuilder;
        private readonly IDictionary<string, Action<string>> _bindings;

        private bool _disposed = false;
        private HubConnection _connection;

        public GenericHubClient(Uri hub, IHubConnectionBuilder hubConnectionBuilder)
        {
            _hub = hub;
            _hubConnectionBuilder = hubConnectionBuilder;

            _bindings = new Dictionary<string, Action<string>>();
        }

        public async Task InitializeHubConnection()
        {
            await GetConnection().ConfigureAwait(false);
        }

        public void RegisterBinding(string calls, Action<string> handleResponse)
        {
            _bindings.Add(calls, handleResponse);
        }

        public Task InvokeAsync(string action, string model)
        {
            return _connection.InvokeAsync(action, model);
        }

        private async Task<HubConnection> GetConnection()
        {
            if (_connection == null)
            {
                _connection = await CreateHubConnectionAsync().ConfigureAwait(false);
            }

            return _connection;
        }

        private async Task<HubConnection> CreateHubConnectionAsync()
        {
            var connection = _hubConnectionBuilder
                .WithUrl(_hub)
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
                .Build();

            BindClientMethods(ref connection);

            await connection
                .StartAsync()
                .ConfigureAwait(false);

            return connection;
        }

        private HubConnection BindClientMethods(ref HubConnection connection)
        {
            foreach (var binding in _bindings)
            {
                connection.On(binding.Key, binding.Value);
            }

            return connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (!(_connection is null))
                    {
                        var result = _connection
                            .DisposeAsync()
                            .Wait(1000);

                        if (!result)
                        {
                            throw new TimeoutException($"Timeout of one second reached while closing hub connection for {_hub}!");
                        }
                    }
                }

                _disposed = true;
            }
        }

        ~GenericHubClient()
        {
            Dispose(false);
        }
    }
}
