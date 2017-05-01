namespace TelegramClient.Core.Network.Exceptions
{
    internal class FileMigrationException : DataCenterMigrationException
    {
        internal FileMigrationException(int dc)
            : base((string)$"File located on a different DC: {dc}.", dc)
        {
        }
    }
}