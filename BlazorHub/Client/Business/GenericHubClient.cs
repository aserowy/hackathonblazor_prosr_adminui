using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorHub.Client.Business
{
    public interface IGenericHubClient : IDisposable
    {
        Task InitializeHubConnection();

        void RegisterBinding(string calls, Type type, Action<string> handleResponse);
        Task InvokeAsync(string action, object model);
    }

    internal class GenericHubClient : IGenericHubClient
    {
        private readonly Uri _hub;
        private readonly IHubConnectionBuilder _hubConnectionBuilder;
        private readonly ILogger<GenericHubClient> _logger;
        private readonly IDictionary<string, (Type Type, Action<string> Action)> _bindings;

        private bool _isInitialized = false;
        private bool _disposed = false;
        private HubConnection _connection;

        public GenericHubClient(Uri hub, IHubConnectionBuilder hubConnectionBuilder, ILogger<GenericHubClient> logger)
        {
            _hub = hub;
            _hubConnectionBuilder = hubConnectionBuilder;
            _logger = logger;

            _bindings = new Dictionary<string, (Type Type, Action<string> Action)>();
        }

        public async Task InitializeHubConnection()
        {
            if (_isInitialized)
            {
                return;
            }

            await GetConnection().ConfigureAwait(false);

            _isInitialized = true;

            _logger.LogInformation($"Hub connection initialized.");
        }

        public void RegisterBinding(string calls, Type type, Action<string> handleResponse)
        {
            _bindings.Add(calls, (type, handleResponse));

            _logger.LogInformation($"Binding for {calls} registered.");
        }

        public Task InvokeAsync(string action, object model)
        {
            _logger.LogInformation($"Action {action} invoked with {JsonSerializer.Serialize(model)}.");

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
                var action = binding.Value.Action;

                connection.On(binding.Key, new[] { binding.Value.Type }, (parameters, state) =>
                {
                    var action = (Action<string>)state;
                    action(JsonSerializer.Serialize(parameters[0]));

                    return Task.CompletedTask;
                }, action);
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
