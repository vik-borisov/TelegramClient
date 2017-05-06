namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    internal interface IResponseResultGetter
    {
        Task<BinaryReader> Recieve(ulong requestId);
    }
}