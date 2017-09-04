namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System.Threading.Tasks;

    internal interface IResponseResultGetter
    {
        Task<object> Recieve(long requestId);
    }
}