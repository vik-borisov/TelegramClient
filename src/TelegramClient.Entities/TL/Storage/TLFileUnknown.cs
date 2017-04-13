using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(-1432995067)]
    public class TlFileUnknown : TlAbsFileType
    {
        public override int Constructor => -1432995067;


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