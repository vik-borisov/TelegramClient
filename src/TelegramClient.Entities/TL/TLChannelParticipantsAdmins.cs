using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1268741783)]
    public class TlChannelParticipantsAdmins : TlAbsChannelParticipantsFilter
    {
        public override int Constructor => -1268741783;


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