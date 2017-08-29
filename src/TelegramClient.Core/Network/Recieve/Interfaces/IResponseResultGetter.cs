namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    using OpenTl.Schema;

    internal interface IResponseResultGetter
    {
        Task<IObject> Recieve(long requestId);
    }
}