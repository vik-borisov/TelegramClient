using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1299865402)]
    public class TlChannelRoleEmpty : TlAbsChannelParticipantRole
    {
        public override int Constructor => -1299865402;


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