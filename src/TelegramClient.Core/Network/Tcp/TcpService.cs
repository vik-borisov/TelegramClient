namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Settings;

    internal class TcpService: IDisposable,
                               ITcpService
    {
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(true);

        private TcpClient _tcpClient;

        public IClientSettings ClientSettings { get; set; }

        private async Task EnsureClientConnected()
        {
            var session = ClientSettings.Session;

            if (_tcpClient == null)
            {
                _resetEvent.WaitOne();
                if (_tcpClient == null)
                {
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(session.ServerAddress, session.Port);

                    _resetEvent.Set();
                }
            }
            else
            {
                var endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;

                 _resetEvent.WaitOne();
                if (!_tcpClient.Connected || endpoint.Address.ToString() != session.ServerAddress || endpoint.Port != session.Port)
                {
                    if (_tcpClient != null)
                    {
                        _tcpClient.Dispose();
                        _tcpClient = null;
                    }
                }
                _resetEvent.Set();
            }
        }

        public async Task Send(byte[] encodedMessage)
        {
            await EnsureClientConnected();
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length);
        }

        public async Task<Stream> Receieve()
        {
            await EnsureClientConnected();

            return _tcpClient.GetStream();
        }

        public void Dispose()
        {
            _tcpClient?.Dispose();
        }
    }
}