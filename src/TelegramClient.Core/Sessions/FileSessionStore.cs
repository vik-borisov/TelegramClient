namespace TelegramClient.Core.Sessions
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(ISessionStore))]
    internal class FileSessionStore : ISessionStore
    {
        private readonly IClientSettings _clientSettings;

        private static readonly ILog Log = LogManager.GetLogger(typeof(FileSessionStore));

        private FileStream _fileStream;

        private readonly object _syncObject = new object();

        public FileSessionStore(IClientSettings clientSettings)
        {
            _clientSettings = clientSettings;
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
                        _fileStream = new FileStream(sessionFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    }
                }

            }
        }

        public void Save()
        {
            Log.Debug($"Save session into ");

            EnsureStreamOpen(_clientSettings.Session.SessionUserId);

            var result = _clientSettings.Session.ToBytes();
            lock (_syncObject)
            {
                _fileStream.Seek(0, SeekOrigin.Begin);
                _fileStream.Write(result, 0, result.Length);
            }
        }

        public ISession Load(string sessionUserId)
        {
            Log.Debug($"Load session for userID = {sessionUserId}");

            EnsureStreamOpen(sessionUserId);

            var buffer = new byte[2048];
            lock (_syncObject)
            {
                _fileStream.Seek(0, SeekOrigin.Begin);

                if (!_fileStream.CanRead)
                {
                    return null;
                }

                _fileStream.Read(buffer, 0, 2048);
            }

            return Session.FromBytes(buffer, sessionUserId);
        }
    }
}