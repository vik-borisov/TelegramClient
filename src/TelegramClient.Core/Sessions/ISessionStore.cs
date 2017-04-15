namespace TelegramClient.Core.Sessions
{
    public interface ISessionStore
    {
        void Save(ISession session);

        ISession Load(string sessionUserId);
    }
}