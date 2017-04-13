using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-496024847)]
    public class TlUserStatusRecently : TlAbsUserStatus
    {
        public override int Constructor => -496024847;


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