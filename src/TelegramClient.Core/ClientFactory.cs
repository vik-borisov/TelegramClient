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
        public static ITelegramClient BuildClient(int appId, string appHash, string serverAddress, int serverPort, string sessionUserId = "session")
        {
            Guard.That(appId).IsPositive();
            Guard.That(appHash).IsNotNullOrWhiteSpace();
            Guard.That(sessionUserId).IsNotNullOrWhiteSpace();
            Guard.That(serverAddress).IsNotNullOrWhiteSpace();
            Guard.That(serverPort).IsPositive();

            var container = CreateContainer();

            FillSettings(container, appId, appHash, sessionUserId, serverAddress, serverPort);

            container.CanGetInstance(typeof(ITelegramClient), string.Empty);
            return container.GetInstance<ITelegramClient>();
        }

        private static void FillSettings(IServiceContainer container, int appId, string appHash, string sessionUserId, string serverAddress, int serverPort)
        {
            Guard.That(appId).IsPositive();
            Guard.That(appHash).IsNotNullOrWhiteSpace();
            Guard.That(sessionUserId).IsNotNullOrWhiteSpace();
            Guard.That(serverAddress).IsNotNullOrWhiteSpace();
            Guard.That(serverPort).IsPositive();

            var settings = container.GetInstance<IClientSettings>();

            settings.AppId = appId;
            settings.AppHash = appHash;

            var store = container.GetInstance<ISessionStore>();
            settings.Session = TryLoadOrCreateNew(store, sessionUserId, serverAddress, serverPort);
        }

        private static IServiceContainer CreateContainer()
        {
            var container = new ServiceContainer();

            container.RegisterInstance<IServiceContainer>(container);
            container.RegisterAssembly(typeof(ClientFactory).GetTypeInfo().Assembly);

            return container;
        }

        private static ISession TryLoadOrCreateNew(ISessionStore store, string sessionUserId, string serverAddress, int serverPort)
        {
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
                                                    ServerAddress = serverAddress,
                                                    Port = serverPort
            };
        }
    }
}