namespace TelegramClient.Core.Network.Tcp
{
    using log4net;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(ITcpService))]
    internal class TcpService : ITcpService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private TcpClient _tcpClient;

        private static readonly ILog Log = LogManager.GetLogger(typeof(TcpService));
        public IClientSettings ClientSettings { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Send(byte[] encodedMessage, CancellationToken cancellationToken)
        {
            await CheckConnectionState().ConfigureAwait(false);
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> Read(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await CheckConnectionState().ConfigureAwait(false);
            return await this._tcpClient.GetStream().ReadAsync(buffer, offset, count);
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _semaphore?.Dispose();
                _tcpClient?.Dispose();
            }
        }

        public async Task Connect()
        {
            this._tcpClient = new TcpClient();
            await this._tcpClient.ConnectAsync(ClientSettings.Session.ServerAddress, ClientSettings.Session.Port).ConfigureAwait(false);
            this._tcpClient.GetStream().ReadTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

        }

        private async Task CheckConnectionState()
        {
            if (!this.IsTcpClientConnected())
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                try
                {
                    if (!this.IsTcpClientConnected())
                    {
                        var previouslyConnected = this._tcpClient != null;
                        await this.Disconnect().ConfigureAwait(false);
                        await this.Connect().ConfigureAwait(false);
                        if (previouslyConnected)
                        {
                            throw new DisconnectedException();
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }

            }
        }

        public bool IsTcpClientConnected()
        {
            if (this._tcpClient == null || !this._tcpClient.Connected ||
                this._tcpClient.Client == null || !this._tcpClient.Client.Connected)
            {
                return false;
            }

            var endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;
            var session = ClientSettings.Session;

            if (endpoint.Address.ToString() != session.ServerAddress || endpoint.Port != session.Port)
            {
                return false;
            }
            return true;
        }

        public Task Disconnect()
        {
            if (this._tcpClient != null)
            {
                _tcpClient?.Dispose();
                _tcpClient = null; 
            }
            return Task.CompletedTask;
        }

        ~TcpService()
        {
            Dispose(false);
        }
    }
}