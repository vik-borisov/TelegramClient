using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-2113143156)]
    public class TlChannelRoleEditor : TlAbsChannelParticipantRole
    {
        public override int Constructor => -2113143156;


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