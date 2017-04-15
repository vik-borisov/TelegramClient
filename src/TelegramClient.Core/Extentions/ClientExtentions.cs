namespace TelegramClient.Core
{
    using LightInject;

    using TelegramClient.Core.Settings;

    internal static class ClientExtentions
    {
        public static IClientSettings GetSettings(this ITelegramClient client)
        {
            return client.Container.GetInstance<IClientSettings>();
        }
    }
}