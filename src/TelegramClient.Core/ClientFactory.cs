namespace TelegramClient.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using BarsGroup.CodeGuard;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
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

        private static void FillSettings(IWindsorContainer container, int appId, string appHash, string sessionUserId, string serverAddress, int serverPort)
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

        private static IWindsorContainer RegisterDependency()
        {
            var container = new WindsorContainer();

            container.RegisterAttibuteRegistration(typeof(ClientFactory).GetTypeInfo().Assembly);

            container.Register(
                Component.For<IDictionary<Type, IRecieveHandler>>().UsingFactoryMethod(
                    kernel =>
                    {
                        var handlerMap = new Dictionary<Type, IRecieveHandler>();

                        var allHandlers = kernel.ResolveAll<IRecieveHandler>();
                        foreach (var handler in allHandlers.ToArray())
                        {
                            foreach (var handleCode in handler.HandleCodes)
                            {
                                handlerMap[handleCode] = handler;
                            }
                        }

                        return handlerMap;
                    }));

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