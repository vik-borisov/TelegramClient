namespace TelegramClient.Core.Network.Exceptions
{
    internal class UserMigrationException : DataCenterMigrationException
    {
        internal UserMigrationException(int dc)
            : base((string)$"User located on a different DC: {dc}.", dc)
        {
        }
    }
}