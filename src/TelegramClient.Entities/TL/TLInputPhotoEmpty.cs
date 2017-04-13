using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(483901197)]
    public class TlInputPhotoEmpty : TlAbsInputPhoto
    {
        public override int Constructor => 483901197;


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