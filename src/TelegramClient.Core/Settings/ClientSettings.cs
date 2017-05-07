namespace TelegramClient.Core.Settings
{
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Sessions;

    [SingleInstance(typeof(IClientSettings))]
    internal class ClientSettings : IClientSettings
    {
        public int AppId { get; set; }

        public string AppHash { get; set; }

        public ISession Session { get; set; }
    }
}