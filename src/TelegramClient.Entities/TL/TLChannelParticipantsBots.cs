using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1328445861)]
    public class TlChannelParticipantsBots : TlAbsChannelParticipantsFilter
    {
        public override int Constructor => -1328445861;


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