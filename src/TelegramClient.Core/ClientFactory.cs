namespace TelegramClient.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using BarsGroup.CodeGuard;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Sessions;
    using TelegramClient.Core.Settings;

    public static class ClientFactory
    {
        public static async Task<ITelegramClient> BuildClient(IFactorySettings factorySettings)
        {
            var container = RegisterDependency();

            await FillSettings(container, factorySettings).ConfigureAwait(false);

            return container.Resolve<ITelegramClient>();
        }

        private static async Task FillSettings(IWindsorContainer container, IFactorySettings factorySettings)
        {
            Guard.That(factorySettings.Id).IsPositive();
            Guard.That(factorySettings.Hash).IsNotNullOrWhiteSpace();
            Guard.That(factorySettings.ServerAddress).IsNotNullOrWhiteSpace();
            Guard.That(factorySettings.ServerPort).IsPositive();
            Guard.That(factorySettings.StoreProvider).IsNotNull();
            
            var settings = container.Resolve<IClientSettings>();

            settings.AppId = factorySettings.Id;
            settings.AppHash = factorySettings.Hash;

            container.Register(Component.For<ISessionStoreProvider>().Instance(factorySettings.StoreProvider));

            var sessionStore = container.Resolve<ISessionStore>();
            
            settings.Session = await TryLoadOrCreateNew(sessionStore, factorySettings).ConfigureAwait(false);
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

        private static async Task<ISession> TryLoadOrCreateNew(ISessionStore sessionStore, IFactorySettings factorySettings)
        {
            var session = await sessionStore.Load().ConfigureAwait(false) ?? Session.Create();
            
            session.ServerAddress = factorySettings.ServerAddress;
            session.Port = factorySettings.ServerPort;

            return session;
        }
    }
}