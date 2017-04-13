using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(-891180321)]
    public class TlFileGif : TlAbsFileType
    {
        public override int Constructor => -891180321;


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