namespace TelegramClient.Core.Network.RecieveHandlers
{
    using System.IO;

    using BarsGroup.CodeGuard;

    using log4net;

    using TelegramClient.Core.ApiServies;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.RecieveHandlers.Interfaces;
    using TelegramClient.Entities;
    using TelegramClient.Entities.TL;

    [SingleInstance(typeof(IRecieveHandler))]
    internal class UpdatesRecieveHandler: IRecieveHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UpdatesRecieveHandler));

        public uint[] HandleCodes { get; } = { 0xd3f45784, 0xe317af7e, 0x2b2fbd4e, 0x78d4dec1, 0x725b04c3, 0x74ae4240 };

        public IUpdatesApiServiceRaiser UpdateRaiser { get; set; }

        public byte[] HandleResponce(uint code, BinaryReader reader)
        {
            Guard.That(reader).IsNotNull();

            var message = ObjectUtils.DeserializeObject(reader, code);

            Log.Debug($"Recieve Updates - {message}");

            var updates = message as TlAbsUpdates;
            Guard.That(updates).IsNotNull();

            UpdateRaiser.OnUpdateRecieve(updates);
            return null;
        }
    }
}