namespace TelegramClient.Core.IoC
{
    using LightInject;

    using TelegramClient.Core.Network;
    using TelegramClient.Core.Sessions;

    using CodeProject.ObjectPool;

    using TelegramClient.Core.Settings;

    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ISessionStore, FileSessionStore>(new PerContainerLifetime());
            serviceRegistry.Register<ITelegramClient, Client>(new PerContainerLifetime());
            serviceRegistry.Register<IMtProtoSender, MtProtoSender>(new PerContainerLifetime());
            serviceRegistry.Register<IMtProtoPlainSender, MtProtoPlainSender>(new PerContainerLifetime());
            serviceRegistry.Register<IClientSettings, ClientSettings>(new PerContainerLifetime());

            RegisterTransportPool(serviceRegistry);
        }

        private static void RegisterTransportPool(IServiceRegistry serviceRegistry)
        {
            IObjectPool<PooledObjectWrapper<ITcpTransport>> Factory (IServiceFactory serviceFactory) {
               return new ObjectPool<PooledObjectWrapper<ITcpTransport>>(10,
                    () => new PooledObjectWrapper<ITcpTransport>(new TcpTransport(serviceFactory.GetInstance<IClientSettings>()))
                );
            }

            serviceRegistry.Register(Factory, new PerContainerLifetime());
        }
    }
}