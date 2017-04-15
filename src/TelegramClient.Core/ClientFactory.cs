namespace TelegramClient.Core
{
    using System;
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

            container.CanGetInstance(typeof(ITelegramClient), String.Empty);
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
            settings.Session = TryLoadOrCreateNew(store, sessionUserId);
        }

        private static ISession TryLoadOrCreateNew(ISessionStore store, string sessionUserId)
        {
            const string DefaultConnectionAddress = "149.154.175.100"; //"149.154.167.50";

            const int DefaultConnectionPort = 443;

            ulong GenerateSessionId()
            {
                var random = new Random();
                var rand = ((ulong)random.Next() << 32) | (ulong)random.Next();
                return rand;
            }

            return store.Load(sessionUserId) ?? new Session
                                                {
                                                    Id = GenerateSessionId(),
                                                    SessionUserId = sessionUserId,
                                                    ServerAddress = DefaultConnectionAddress,
                                                    Port = DefaultConnectionPort
                                                };
        }
    }
}