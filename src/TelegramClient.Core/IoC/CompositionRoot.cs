namespace TelegramClient.Core.IoC
{
    using LightInject;

    using TelegramClient.Core.Network;
    using TelegramClient.Core.Sessions;

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
            serviceRegistry.Register<ITcpTransport, TcpTransport>(new PerContainerLifetime());
        }
    }
}