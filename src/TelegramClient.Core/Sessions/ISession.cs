namespace TelegramClient.Core.Sessions
{
    using System;

    using OpenTl.Schema;

    using TelegramClient.Core.MTProto.Crypto;

    internal interface ISession
    {
        string SessionUserId { get; set; }

        string ServerAddress { get; set; }

        int Port { get; set; }

        AuthKey AuthKey { get; set; }

        ulong Id { get; set; }

        long Salt { get; set; }

        int TimeOffset { get; set; }

        int SessionExpires { get; set; }

        TUser User { get; set; }

        long GenerateMsgId();

        Tuple<long, int> GenerateMsgIdAndSeqNo(bool confirmed);

        byte[] ToBytes();
    }
}