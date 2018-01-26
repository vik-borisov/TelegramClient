namespace TelegramClient.Core.Network.Tcp
{
	using System;
	using System.IO;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;
	using System.Threading.Tasks;

	using TelegramClient.Core.IoC;
	using TelegramClient.Core.Settings;
	using TelegramClient.Core.Helpers;

	[SingleInstance(typeof(ITcpService))]
	internal class TcpService : ITcpService
	{
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		private TcpClient _tcpClient;

		public IClientSettings ClientSettings { get; set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public async Task<Stream> Receieve()
		{
			await EnsureClientConnected().ConfigureAwait(false); 
			return _tcpClient.GetStream();
		}

		public async Task Send(byte[] encodedMessage)
		{
			await EnsureClientConnected().ConfigureAwait(false);
			await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length).ConfigureAwait(false);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_semaphore?.Dispose();
				_tcpClient?.Dispose();
			}
		}

		private async Task Reconnect()
		{
			var session = ClientSettings.Session;
			_tcpClient = new TcpClient();
			await _tcpClient.ConnectAsync(session.ServerAddress, session.Port).ConfigureAwait(false);
		}

		private async Task EnsureClientConnected()
		{
			if (_tcpClient == null)
			{
				await _semaphore.WaitAsync().ConfigureAwait(false);
				if (_tcpClient == null)
				{
					await this.Reconnect().ConfigureAwait(false);
				}
				_semaphore.Release();
			}
			else
			{
				if (!_tcpClient.IsConnected())
				{
					await _semaphore.WaitAsync();
					var endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;
					var session = ClientSettings.Session;

					if (!_tcpClient.IsConnected() || endpoint.Address.ToString() != session.ServerAddress || endpoint.Port != session.Port)
					{
						_tcpClient.Dispose();
						_tcpClient = null;

						await this.Reconnect().ConfigureAwait(false);
					}

					_semaphore.Release();
				}
			}
		}

		~TcpService()
		{
			Dispose(false);
		}
	}
}