using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1038136962)]
    public class TlEncryptedFileEmpty : TlAbsEncryptedFile
    {
        public override int Constructor => -1038136962;


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