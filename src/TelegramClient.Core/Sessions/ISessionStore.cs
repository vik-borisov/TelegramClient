namespace TelegramClient.Core.Sessions
{
    internal interface ISessionStore
    {
        ISession Load(string sessionUserId);

        void Save();
    }
}