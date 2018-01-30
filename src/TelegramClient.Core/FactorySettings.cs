namespace TelegramClient.Core
{
    using TelegramClient.Core.Sessions;

    public interface IFactorySettings
    {
        int Id { get; set; }

        string Hash { get; set; }

        string ServerAddress { get; set; }

        int ServerPort { get; set; }

        ISessionStoreProvider StoreProvider { get; set; }
    }

    public class FactorySettings : IFactorySettings
    {
        public int Id { get; set; }

        public string Hash { get; set; }

        public string ServerAddress { get; set; }

        public int ServerPort { get; set; }

        public ISessionStoreProvider StoreProvider { get; set; }
    }
}