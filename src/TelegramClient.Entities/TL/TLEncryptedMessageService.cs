using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(594758406)]
    public class TlEncryptedMessageService : TlAbsEncryptedMessage
    {
        public override int Constructor => 594758406;

        public long RandomId { get; set; }
        public int ChatId { get; set; }
        public int Date { get; set; }
        public byte[] Bytes { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            RandomId = br.ReadInt64();
            ChatId = br.ReadInt32();
            Date = br.ReadInt32();
            Bytes = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(RandomId);
            bw.Write(ChatId);
            bw.Write(Date);
            BytesUtil.Serialize(Bytes, bw);
        }
    }
}