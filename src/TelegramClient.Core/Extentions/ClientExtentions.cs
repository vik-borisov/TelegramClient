namespace TelegramClient.Core
{
    using Autofac;

    using TelegramClient.Core.Settings;

    internal static class ClientExtentions
    {
        public static IClientSettings GetSettings(this ITelegramClient client)
        {
            return client.Container.Resolve<IClientSettings>();
        }
    }
}