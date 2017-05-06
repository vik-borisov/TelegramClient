namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using log4net;

    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.Interfaces;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Settings;

    internal class BadServerSaltRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BadServerSaltRecieveHandler));

        public uint ResponceCode { get; } = 0xedab447b;

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public IEnumerable<byte[]> HandleResponce(BinaryReader reader)
        {
            var code = reader.ReadUInt32();
            var badMsgId = reader.ReadUInt64();
            var badMsgSeqNo = reader.ReadInt32();
            var errorCode = reader.ReadInt32();
            var newSalt = reader.ReadUInt64();

            Log.Info($"Bad server sault detected! message id = {badMsgId} ");

            ClientSettings.Session.Salt = newSalt;

            ConfirmationRecieveService.RequestWithException(badMsgId, new BadServerSaltException());

            return Enumerable.Empty<byte[]>();
        }
    }
}