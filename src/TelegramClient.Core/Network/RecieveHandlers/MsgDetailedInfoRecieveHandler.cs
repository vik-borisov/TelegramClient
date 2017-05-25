namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using log4net;

    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class MsgDetailedInfoRecieveHandler : IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PingRecieveHandler));

        public int[] HandleCodes { get; } = { 0x276d3ec6 };

        public byte[] HandleResponce(int code, BinaryReader reader)
        {
            Log.Debug("Handle MsgDetailedInfo");

            return null;
        }
    }
}