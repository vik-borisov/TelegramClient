using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2011940674)]
    public class TlUserStatusLastMonth : TlAbsUserStatus
    {
        public override int Constructor => 2011940674;


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