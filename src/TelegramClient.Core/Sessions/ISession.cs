namespace TelegramClient.Core.Sessions
{
    using global::TelegramClient.Core.MTProto.Crypto;
    using global::TelegramClient.Entities.TL;

    public interface ISession
    {
        string SessionUserId { get; set; }

        string ServerAddress { get; set; }

        int Port { get; set; }

        AuthKey AuthKey { get; set; }

        ulong Id { get; set; }

        ulong Salt { get; set; }

        int TimeOffset { get; set; }

        int SessionExpires { get; set; }

        TlUser TlUser { get; set; }

        byte[] ToBytes();

        long GetNewMessageId();

        int GenerateSequence(bool confirmed);
    }
}