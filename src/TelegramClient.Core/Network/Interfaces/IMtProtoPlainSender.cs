namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    internal interface IMtProtoPlainSender
    {
        Task<byte[]> SendAndReceive(byte[] data, CancellationToken cancellationToken = default(CancellationToken));
    }
}