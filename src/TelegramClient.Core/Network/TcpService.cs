namespace TelegramClient.Core.Network
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using TelegramClient.Core.Settings;

    internal class TcpService: IDisposable,
                               ITcpService
    {
        private TcpClient _tcpClient;

        public IClientSettings ClientSettings { get; set; }

        private async Task EnsureClientConnected()
        {
            var session = ClientSettings.Session;

            if (_tcpClient != null)
            {
                var endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;

                if (_tcpClient.Connected && endpoint.Address.ToString() == session.ServerAddress && endpoint.Port == session.Port)
                {
                    return;
                }

                _tcpClient.Dispose();
            }

            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(session.ServerAddress, session.Port);
        }

        public async Task Send(byte[] encodedMessage)
        {
            await EnsureClientConnected();
            await _tcpClient.GetStream().WriteAsync(encodedMessage, 0, encodedMessage.Length);
        }

        public async Task<NetworkStream> Receieve()
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