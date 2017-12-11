namespace TelegramClient.Core.Network.Tcp
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    internal interface ITcpService: IDisposable
    {
        Task Send(byte[] encodedMessage);

        Task<Stream> Receieve();
    }
}