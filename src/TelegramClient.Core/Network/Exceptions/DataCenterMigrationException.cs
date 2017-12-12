namespace TelegramClient.Core.Network.Exceptions
{
    using System;

    internal abstract class DataCenterMigrationException : Exception
    {
        private const string ReportMessage =
            " See: https://github.com/sochix/TLSharp#i-get-a-xxxmigrationexception-or-a-migrate_x-error";

        internal int Dc { get; }

        protected DataCenterMigrationException(string msg, int dc) : base(msg + ReportMessage)
        {
            Dc = dc;
        }
    }
}