using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-317144808)]
    public class TlEncryptedMessage : TlAbsEncryptedMessage
    {
        public override int Constructor => -317144808;

        public long RandomId { get; set; }
        public int ChatId { get; set; }
        public int Date { get; set; }
        public byte[] Bytes { get; set; }
        public TlAbsEncryptedFile File { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            RandomId = br.ReadInt64();
            ChatId = br.ReadInt32();
            Date = br.ReadInt32();
            Bytes = BytesUtil.Deserialize(br);
            File = (TlAbsEncryptedFile) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(RandomId);
            bw.Write(ChatId);
            bw.Write(Date);
            BytesUtil.Serialize(Bytes, bw);
            ObjectUtils.SerializeObject(File, bw);
        }
    }
}