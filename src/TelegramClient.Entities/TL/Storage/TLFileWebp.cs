using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(276907596)]
    public class TlFileWebp : TlAbsFileType
    {
        public override int Constructor => 276907596;


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