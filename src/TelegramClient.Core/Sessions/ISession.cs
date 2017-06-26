namespace TelegramClient.Core.Sessions
{
    using System;

    using global::TelegramClient.Core.MTProto.Crypto;

    using OpenTl.Schema;

    internal interface ISession
    {
        string SessionUserId { get; set; }

        string ServerAddress { get; set; }

        int Port { get; set; }

        AuthKey AuthKey { get; set; }

        ulong Id { get; set; }

        ulong Salt { get; set; }

        int TimeOffset { get; set; }

        int SessionExpires { get; set; }

        TUser User { get; set; }

        byte[] ToBytes();

        Tuple<ulong, int> GenerateMsgIdAndSeqNo(bool confirmed);

        ulong GenerateMsgId();
    }
}