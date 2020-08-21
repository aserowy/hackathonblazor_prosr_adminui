// Generated with prosr1 by compiler 0.9, template csharp.tmpl 0.1

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prosr.Hub
{
	public interface IHelloWorlHubClient : IHelloWorlHubClientBase, IDisposable
	{
		Task InitializeHubConnection();
		Task CallGreetAllOnHub(HelloRequest message);
		Task CallGreetCallerOnHub(HelloRequest message);
	}

	public interface IHelloWorlHubClientBase
	{
		Task InformGreetCall(HelloRequest message);
		Task HandleGreetingsToAll(HelloResponse message);
		Task HandleGreetings(HelloResponse message);
	}

	public abstract class HelloWorlHubClientBase : IHelloWorlHubClient
	{
		private readonly IHubConnectionBuilder _hubConnectionBuilder;

		private bool _disposed = false;
		private HubConnection _connection;

		protected HelloWorlHubClientBase(IHubConnectionBuilder hubConnectionBuilder)
		{
			_hubConnectionBuilder = hubConnectionBuilder;
		}

		protected abstract Uri HubUrl { get; }

		public async Task InitializeHubConnection()
		{
			await GetConnection().ConfigureAwait(false);
		}

		public abstract Task InformGreetCall(HelloRequest message);

		public abstract Task HandleGreetingsToAll(HelloResponse message);

		public abstract Task HandleGreetings(HelloResponse message);

		public async Task CallGreetAllOnHub(HelloRequest message)
		{
			var connection = await GetConnection().ConfigureAwait(false);

			await connection
				.SendAsync("GreetAll", message)
				.ConfigureAwait(false);
		}

		public async Task CallGreetCallerOnHub(HelloRequest message)
		{
			var connection = await GetConnection().ConfigureAwait(false);

			await connection
				.SendAsync("GreetCaller", message)
				.ConfigureAwait(false);
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
				.WithUrl(HubUrl)
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
			connection.On<HelloRequest>("InformGreetCall", message => InformGreetCall(message));
			connection.On<HelloResponse>("HandleGreetingsToAll", message => HandleGreetingsToAll(message));
			connection.On<HelloResponse>("HandleGreetings", message => HandleGreetings(message));

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
							throw new TimeoutException($"Timeout of one second reached while closing hub connection for {HubUrl}!");
						}
					}
				}

				_disposed = true;
			}
		}
		
		~HelloWorlHubClientBase()
		{
			Dispose(false);
		}
	}

	public abstract class HelloWorlHubBase : Hub<IHelloWorlHubClientBase>
	{
		protected Task SendInformGreetCallOnAllAsync(HelloRequest message)
		{
			return Clients.All.InformGreetCall(message);
		}

		public async Task GreetAll(HelloRequest message)
		{
			var result = await HandleGreetAllAsync(message).ConfigureAwait(false);

			await Clients
				.All
				.HandleGreetingsToAll(result)
				.ConfigureAwait(false);
		}

		protected abstract Task<HelloResponse> HandleGreetAllAsync(HelloRequest message);
		
		public async Task GreetCaller(HelloRequest message)
		{
			var result = await HandleGreetCallerAsync(message).ConfigureAwait(false);

			await Clients
				.Caller
				.HandleGreetings(result)
				.ConfigureAwait(false);
		}

		protected abstract Task<HelloResponse> HandleGreetCallerAsync(HelloRequest message);
		
	}

	public class HelloRequest
	{
		public string Name { get; set; }
	}

	public class HelloResponse
	{
		public HelloRequest Request { get; set; }
		public string ServerMessage { get; set; }
	}
}
	