namespace TelegramClient.Core.Sessions
{
    using System.Threading.Tasks;

    internal interface ISessionStore
    {
        Task<ISession> Load();

        Task Save();

        Task Remove();
    }
}