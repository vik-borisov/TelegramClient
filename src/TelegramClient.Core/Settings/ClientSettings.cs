namespace TelegramClient.Core.Settings
{
    using TelegramClient.Core.Sessions;

    public class ClientSettings : IClientSettings
    {
        public int AppId { get; set; }

        public string AppHash { get; set; }

        public ISession Session { get; set; }
    }
}