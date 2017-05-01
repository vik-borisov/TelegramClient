using System.IO;

namespace TelegramClient.Entities
{
    public abstract class TlMethod : TlObject
    {
        public abstract void DeserializeResponse(BinaryReader stream);

        #region MTPROTO

        public ulong MessageId { get; set; }
        public virtual bool Confirmed { get; } = true;

        #endregion
    }
}