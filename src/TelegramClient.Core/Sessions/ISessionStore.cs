namespace TelegramClient.Core.Sessions
{
    internal interface ISessionStore
    {
        void Save();

        ISession Load(string sessionUserId);
    }
}