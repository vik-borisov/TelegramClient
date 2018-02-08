namespace TelegramClient.Core.Sessions
{
    using System;
    using System.Threading.Tasks;

    using OpenTl.Schema;

    using TelegramClient.Core.MTProto.Crypto;

    internal interface ISession
    {
        string ServerAddress { get; set; }

        int Port { get; set; }

        AuthKey AuthKey { get; set; }

        ulong Id { get; set; }

        long Salt { get; set; }

        int TimeOffset { get; set; }

        int SessionExpires { get; set; }

        TUser User { get; set; }

        long GenerateMsgId();

        Task<(long, int)> GenerateMsgIdAndSeqNo(bool confirmed);

        byte[] ToBytes();
    }
}