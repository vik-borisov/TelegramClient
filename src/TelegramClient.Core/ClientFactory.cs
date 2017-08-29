namespace TelegramClient.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Autofac;

    using BarsGroup.CodeGuard;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.Recieve;
    using TelegramClient.Core.Network.Recieve.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Network.Tcp;
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

            var container = RegisterDependency();

            FillSettings(container, appId, appHash, sessionUserId, serverAddress, serverPort);

            return container.Resolve<ITelegramClient>();
        }

        private static void FillSettings(IContainer container, int appId, string appHash, string sessionUserId, string serverAddress, int serverPort)
        {
            Guard.That(appId).IsPositive();
            Guard.That(appHash).IsNotNullOrWhiteSpace();
            Guard.That(sessionUserId).IsNotNullOrWhiteSpace();
            Guard.That(serverAddress).IsNotNullOrWhiteSpace();
            Guard.That(serverPort).IsPositive();

            var settings = container.Resolve<IClientSettings>();

            settings.AppId = appId;
            settings.AppHash = appHash;

            var store = container.Resolve<ISessionStore>();
            settings.Session = TryLoadOrCreateNew(store, sessionUserId, serverAddress, serverPort);
        }

        private static IContainer RegisterDependency()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAttibuteRegistration(typeof(ClientFactory).GetTypeInfo().Assembly);

            builder.RegisterAssemblyTypes(typeof(ClientFactory).GetTypeInfo().Assembly)
                   .Where(typeof(IRecieveHandler).IsAssignableFrom)
                   .As<IRecieveHandler>()
                   .SingleInstance()
                   .PropertiesAutowired();
            builder.RegisterAdapter<IEnumerable<IRecieveHandler>, Dictionary<Type, IRecieveHandler>>(handlers =>
            {
                var handlerMap = new Dictionary<Type, IRecieveHandler>();
                foreach (var handler in handlers.ToArray())
                {
                    foreach (var handleCode in handler.HandleCodes)
                    {
                        handlerMap[handleCode] = handler;
                    }
                }

                return handlerMap;
            });

           return builder.Build();
        }

    private static ISession TryLoadOrCreateNew(ISessionStore store, string sessionUserId, string serverAddress, int serverPort)
        {
            ulong GenerateSessionId()
            {
                var random = new Random();
                var rand = ((ulong)random.Next() << 32) | (ulong)random.Next();
                return rand;
            }

            var session = store.Load(sessionUserId) ?? new Session
                                                       {
                                                           Id = GenerateSessionId(),
                                                           SessionUserId = sessionUserId
                                                       };
            session.ServerAddress = serverAddress;
            session.Port = serverPort;

            return session;
        }
    }
}