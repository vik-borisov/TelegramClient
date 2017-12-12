namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    internal interface ITcpService : IDisposable
    {
        Task<Stream> Receieve();

        Task Send(byte[] encodedMessage);
    }
}