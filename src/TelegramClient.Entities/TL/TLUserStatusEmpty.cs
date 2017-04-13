using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(164646985)]
    public class TlUserStatusEmpty : TlAbsUserStatus
    {
        public override int Constructor => 164646985;


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