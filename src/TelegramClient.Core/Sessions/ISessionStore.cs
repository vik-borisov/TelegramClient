namespace TelegramClient.Core.Sessions
{
    internal interface ISessionStore
    {
        void Save(ISession session);

        ISession Load(string sessionUserId);
    }
}