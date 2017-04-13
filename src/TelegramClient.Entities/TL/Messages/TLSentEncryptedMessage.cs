using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1443858741)]
    public class TlSentEncryptedMessage : TlAbsSentEncryptedMessage
    {
        public override int Constructor => 1443858741;

        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Date);
        }
    }
}