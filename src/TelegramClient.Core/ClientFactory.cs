namespace TelegramClient.Core
{
    using System.Reflection;

    using BarsGroup.CodeGuard;
    using BarsGroup.CodeGuard.Validators;

    using LightInject;

    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    public static class ClientFactory
    {
        public static ITelegramClient GetClient(int appId, string appHash, string sessionUserId = "session")
        {
            Guard.That(appId).IsPositive();
            Guard.That(appHash).IsNotNullOrWhiteSpace();
            Guard.That(sessionUserId).IsNotNullOrWhiteSpace();

            var container = CreateContainer();

            FillSettings(container, appId, appHash, sessionUserId);

            container.CanGetInstance(typeof(ITelegramClient), string.Empty);
            return container.GetInstance<ITelegramClient>();
        }

        private static IServiceContainer CreateContainer()
        {
            var container = new ServiceContainer();

            container.RegisterInstance<IServiceContainer>(container);
            container.RegisterAssembly(typeof(ClientFactory).GetTypeInfo().Assembly);

            return container;
        }

        private static void FillSettings(IServiceContainer container,  int appId, string appHash, string sessionUserId)
        {
            Guard.That(appId).IsPositive();
            Guard.That(appHash).IsNotNullOrWhiteSpace();
            Guard.That(sessionUserId).IsNotNullOrWhiteSpace();

            var settings = container.GetInstance<IClientSettings>();

            settings.AppId = appId;
            settings.AppHash = appHash;

            var store = container.GetInstance<ISessionStore>();
            settings.Session = store.Load(sessionUserId);
        }
    }
}