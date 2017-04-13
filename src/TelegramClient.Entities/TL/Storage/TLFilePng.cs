using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(172975040)]
    public class TlFilePng : TlAbsFileType
    {
        public override int Constructor => 172975040;


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