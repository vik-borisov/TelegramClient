namespace TelegramClient.Core.Responces
{
    using System.IO;

    public class PongResponce : BaseResponce
    {
        public override uint Constructor => 0x347773c5;

        public override void SerializeBody(BinaryWriter writer)
        {
        }

        public PongResponce(ulong requestMessageId) : base(requestMessageId)
        {
        }
    }
}