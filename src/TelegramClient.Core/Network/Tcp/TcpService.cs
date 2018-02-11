namespace TelegramClient.Core.Network.Tcp
{
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

        public IClientSettings ClientSettings { get; set; }

        private async Task Connect()
        {
            _tcpClient = new TcpClient();
            
            await _tcpClient.ConnectAsync(ClientSettings.Session.ServerAddress, ClientSettings.Session.Port).ConfigureAwait(false);
            
            _tcpClient.GetStream().ReadTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
        }

        public Task Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient?.Dispose();
                _tcpClient = null;
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool IsTcpClientConnected()
        {
            if (_tcpClient == null || !_tcpClient.Connected ||
                _tcpClient.Client == null || !_tcpClient.Client.Connected)
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

        public async Task<int> Read(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await CheckConnectionState().ConfigureAwait(false);
            
            return await _tcpClient.GetStream().ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
        }

        public async Task Send(byte[] encodedMessage, CancellationToken cancellationToken)
        {
            await CheckConnectionState().ConfigureAwait(false);
            
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length, cancellationToken).ConfigureAwait(false);
        }

        private async Task CheckConnectionState()
        {
            if (!IsTcpClientConnected())
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                
                try
                {
                    if (!IsTcpClientConnected())
                    {
                        var previouslyConnected = _tcpClient != null;
                        
                        await Disconnect().ConfigureAwait(false);
                        
                        await Connect().ConfigureAwait(false);
                        
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

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _semaphore?.Dispose();
                _tcpClient?.Dispose();
            }
        }

        ~TcpService()
        {
            Dispose(false);
        }
    }
}