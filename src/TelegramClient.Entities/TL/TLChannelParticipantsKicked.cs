using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1010285434)]
    public class TlChannelParticipantsKicked : TlAbsChannelParticipantsFilter
    {
        public override int Constructor => 1010285434;


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