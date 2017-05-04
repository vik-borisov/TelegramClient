namespace TelegramClient.Core.Network.Interfaces
{
    using System.Threading.Tasks;

    internal interface IMtProtoPlainSender
    {
        Task<byte[]> SendAndReceive(byte[] data);
    }
}