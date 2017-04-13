using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1802240206)]
    public class TlSentEncryptedFile : TlAbsSentEncryptedMessage
    {
        public override int Constructor => -1802240206;

        public int Date { get; set; }
        public TlAbsEncryptedFile File { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Date = br.ReadInt32();
            File = (TlAbsEncryptedFile) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Date);
            ObjectUtils.SerializeObject(File, bw);
        }
    }
}