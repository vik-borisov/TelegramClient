namespace TelegramClient.Core.Network
{
    using System.Threading.Tasks;

    internal interface IMtProtoPlainSender
    {
        Task<byte[]> SendAndReceive(byte[] data);
    }
}