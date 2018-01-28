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

        public async Task<NetworkStream> Receieve()
        {
            await EnsureClientConnected().ConfigureAwait(false);
            return _tcpClient.GetStream();
        }

        public async Task Send(byte[] encodedMessage)
        {
            await EnsureClientConnected().ConfigureAwait(false);
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length).ConfigureAwait(false);
            _tcpClient.GetStream().Flush();
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
            this._tcpClient = new TcpClient();
            await this._tcpClient.ConnectAsync(ClientSettings.Session.ServerAddress, ClientSettings.Session.Port).ConfigureAwait(false);
            this._tcpClient.GetStream().WriteTimeout = 500;
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
                if (!this.IsTcpClientConnected())
                {
                    await _semaphore.WaitAsync().ConfigureAwait(false);
                    var endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;
                    var session = ClientSettings.Session;

                    if (!this.IsTcpClientConnected() || endpoint.Address.ToString() != session.ServerAddress || endpoint.Port != session.Port)
                    {
                        _tcpClient.Dispose();
                        _tcpClient = null;
                        await this.Reconnect().ConfigureAwait(false);
                    }
                    _semaphore.Release();
                }
            }
        }

        public bool IsTcpClientConnected()
        {
            if (this._tcpClient == null || !(this._tcpClient.Connected || (this._tcpClient.Client == null || !(this._tcpClient.Client.Connected)
            {
                return false;
            }

            if (this._tcpClient.Client.Poll(0, SelectMode.SelectRead))
            {
                byte[] buff = new byte[1];
                if (this._tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        ~TcpService()
        {
            Dispose(false);
        }
    }
}