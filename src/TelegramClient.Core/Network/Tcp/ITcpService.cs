namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    internal interface ITcpService : IDisposable
    {
        Task<NetworkStream> Receieve(); 

        Task Send(byte[] encodedMessage);
    }
}