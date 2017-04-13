using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(1384777335)]
    public class TlFileMp3 : TlAbsFileType
    {
        public override int Constructor => 1384777335;


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