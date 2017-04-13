using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(1258941372)]
    public class TlFileMov : TlAbsFileType
    {
        public override int Constructor => 1258941372;


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