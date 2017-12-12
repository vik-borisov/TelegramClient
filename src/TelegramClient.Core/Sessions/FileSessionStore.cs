namespace TelegramClient.Core.Sessions
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(ISessionStore))]
    internal class FileSessionStore : ISessionStore
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileSessionStore));

        private readonly object _syncObject = new object();

        private FileStream _fileStream;

        public IClientSettings ClientSettings { get; set; }

        public ISession Load(string sessionUserId)
        {
            Log.Debug($"Load session for userID = {sessionUserId}");

            EnsureStreamOpen(sessionUserId);

            var buffer = new byte[2048];
            lock (_syncObject)
            {
                _fileStream.Position = 0;

                if (_fileStream.Length == 0)
                {
                    return null;
                }

                _fileStream.Read(buffer, 0, 2048);
            }

            return Session.FromBytes(buffer, sessionUserId);
        }

        public void Save()
        {
            Log.Debug($"Save session into ");

            EnsureStreamOpen(ClientSettings.Session.SessionUserId);

            var result = ClientSettings.Session.ToBytes();
            lock (_syncObject)
            {
                _fileStream.Position = 0;
                _fileStream.Write(result, 0, result.Length);
                _fileStream.Flush();
            }
        }

        private void EnsureStreamOpen(string sessionUserId)
        {
            var sessionFile = $"{sessionUserId}.dat";

            if (_fileStream == null)
            {
                lock (_syncObject)
                {
                    if (_fileStream == null)
                    {
                        //TODO: reuse stream with a different session size
                        _fileStream = new FileStream(sessionFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    }
                }
            }
        }
    }
}