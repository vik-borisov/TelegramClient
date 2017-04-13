using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(767652808)]
    public class TlInputEncryptedFileBigUploaded : TlAbsInputEncryptedFile
    {
        public override int Constructor => 767652808;

        public long Id { get; set; }
        public int Parts { get; set; }
        public int KeyFingerprint { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
            Parts = br.ReadInt32();
            KeyFingerprint = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(Parts);
            bw.Write(KeyFingerprint);
        }
    }
}