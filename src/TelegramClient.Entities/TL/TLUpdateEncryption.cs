using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1264392051)]
    public class TlUpdateEncryption : TlAbsUpdate
    {
        public override int Constructor => -1264392051;

        public TlAbsEncryptedChat Chat { get; set; }
        public int Date { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Chat = (TlAbsEncryptedChat) ObjectUtils.DeserializeObject(br);
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Chat, bw);
            bw.Write(Date);
        }
    }
}