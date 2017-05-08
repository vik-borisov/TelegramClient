using System.IO;

namespace TelegramClient.Entities
{
    public abstract class TlMethod : TlObject
    {
        public abstract void DeserializeResponse(BinaryReader stream);

        public virtual bool Confirmed { get; } = true;
    }
}