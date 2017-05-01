namespace TelegramClient.Core.Network.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    internal interface IMtProtoReciever
    {
        Task<BinaryReader> Recieve(ulong requestId);
    }
}