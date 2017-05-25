namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Confirm;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Core.Settings;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class BadServerSaltRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BadServerSaltRecieveHandler));

        public int[] HandleCodes { get; } = { -307542917 };

        public IConfirmationRecieveService ConfirmationRecieveService { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            var badMsgId = reader.ReadUInt64();
            var badMsgSeqNo = reader.ReadInt32();
            var errorCode = reader.ReadInt32();
            var newSalt = reader.ReadUInt64();

            Log.Info($"Bad server sault detected! message id = {badMsgId} ");

            ClientSettings.Session.Salt = newSalt;

            ConfirmationRecieveService.RequestWithException(badMsgId, new BadServerSaltException());

            return null;
        }
    }
}