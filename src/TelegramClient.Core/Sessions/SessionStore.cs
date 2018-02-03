namespace TelegramClient.Core.Sessions
{
    using System.Threading.Tasks;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(ISessionStore))]
    internal class SessionStore : ISessionStore
    {
        public IClientSettings ClientSettings { get; set; }

        public ISessionStoreProvider StoreProvider { get; set; }

        public async Task<ISession> Load()
        {
            var data = await StoreProvider.LoadSession().ConfigureAwait(false);
            return Session.FromBytes(data);
        }

        public async Task Remove()
        {
            await StoreProvider.RemoveSession().ConfigureAwait(false);
        }

        public async Task Save()
        {
            var session = ClientSettings.Session.ToBytes();
            await StoreProvider.SaveSession(session).ConfigureAwait(false);
        }
    }
}