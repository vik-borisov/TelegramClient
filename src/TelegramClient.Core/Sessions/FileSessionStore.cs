namespace TelegramClient.Core.Sessions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class FileSessionStore : ISessionStore
    {
        private readonly Object _syncObject = new object();

        private Task _saveTask;

        public void Save(ISession session)
        {
            if (_saveTask == null)
            {
                lock (_syncObject)
                {
                    if (_saveTask == null)
                    {
                        _saveTask = Task.Delay(500)
                                       .ContinueWith(_ => SaveToFile(session))
                                       .ContinueWith( _ => _saveTask = null);
                    }
                }
            }
        }

        private static void SaveToFile(ISession session)
        {
            using (var stream = new FileStream($"{session.SessionUserId}.dat", FileMode.OpenOrCreate))
            {
                var result = session.ToBytes();
                stream.Write(result, 0, result.Length);
            }
        }

        public ISession Load(string sessionUserId)
        {
            var sessionFileName = $"{sessionUserId}.dat";
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