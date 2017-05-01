namespace TelegramClient.Core.Network.Exceptions
{
    internal class PhoneMigrationException : DataCenterMigrationException
    {
        internal PhoneMigrationException(int dc)
            : base((string)$"Phone number registered to a different DC: {dc}.", dc)
        {
        }
    }
}