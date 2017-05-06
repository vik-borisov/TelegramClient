namespace TelegramClient.Core.Responces
{
    using System;
    using System.IO;

    public abstract class BaseResponce
    {
        private readonly ulong _requestMessageId;

        public abstract uint Constructor { get;}

        protected BaseResponce(ulong requestMessageId)
        {
            _requestMessageId = requestMessageId;
        }

        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(Constructor);
            writer.Write(_requestMessageId);
            SerializeBody(writer);
        }
            
        public abstract void SerializeBody(BinaryWriter writer);
    }
}