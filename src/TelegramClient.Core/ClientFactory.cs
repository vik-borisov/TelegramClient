namespace TelegramClient.Core
{
    using System;

    using Autofac;

    using BarsGroup.CodeGuard;

    using CodeProject.ObjectPool;

    using TelegramClient.Core.Network;
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

            builder.RegisterType<FileSessionStore>().As<ISessionStore>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<Client>().As<ITelegramClient>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<MtProtoSender>().As<IMtProtoSender>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<MtProtoPlainSender>().As<IMtProtoPlainSender>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<ClientSettings>().As<IClientSettings>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<TcpTransport>().As<ITcpTransport>().InstancePerDependency().PropertiesAutowired();
            builder.RegisterType<TcpService>().As<ITcpService>().InstancePerDependency().PropertiesAutowired();

            builder.Register<IObjectPool<PooledObjectWrapper<ITcpTransport>>>(
                       context =>
                       {
                           var transport = context.Resolve<ITcpTransport>();
                           return new ObjectPool<PooledObjectWrapper<ITcpTransport>>(10, () => new PooledObjectWrapper<ITcpTransport>(transport));
                       })
                .InstancePerDependency()
                .PropertiesAutowired();

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