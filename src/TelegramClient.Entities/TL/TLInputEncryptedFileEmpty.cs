using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(406307684)]
    public class TlInputEncryptedFileEmpty : TlAbsInputEncryptedFile
    {
        public override int Constructor => 406307684;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}