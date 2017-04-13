using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(129960444)]
    public class TlUserStatusLastWeek : TlAbsUserStatus
    {
        public override int Constructor => 129960444;


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