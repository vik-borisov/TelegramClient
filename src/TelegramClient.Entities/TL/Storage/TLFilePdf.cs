using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(-1373745011)]
    public class TlFilePdf : TlAbsFileType
    {
        public override int Constructor => -1373745011;


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