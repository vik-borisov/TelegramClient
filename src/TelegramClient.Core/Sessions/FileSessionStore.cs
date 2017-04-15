namespace TelegramClient.Core.Sessions
{
    using System.IO;

    public class FileSessionStore : ISessionStore
    {
        public void Save(ISession session)
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