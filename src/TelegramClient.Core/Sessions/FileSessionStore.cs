namespace TelegramClient.Core.Sessions
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(ISessionStore))]
    internal class FileSessionStore : ISessionStore
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileSessionStore));

        public void Save(ISession session)
        {
            var sessionFile = $"{session.SessionUserId}.dat";

            Log.Debug($"Save session into file '{sessionFile}'");

            using (var stream = new FileStream(sessionFile, FileMode.OpenOrCreate))
            {
                var result = session.ToBytes();
                stream.Write(result, 0, result.Length);
            }
        }

        public ISession Load(string sessionUserId)
        {
            var sessionFileName = $"{sessionUserId}.dat";

            Log.Debug($"Load session from file '{sessionFileName}'");

            if (!File.Exists(sessionFileName))
                return null;

            using (var stream = new FileStream(sessionFileName, FileMode.Open))
            {
                var buffer = new byte[2048];
                stream.Read(buffer, 0, 2048);

                return Session.FromBytes(buffer, sessionUserId);
            }
        }
    }
}