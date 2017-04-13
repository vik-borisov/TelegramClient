using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1776756363)]
    public class TlChannelRoleModerator : TlAbsChannelParticipantRole
    {
        public override int Constructor => -1776756363;


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