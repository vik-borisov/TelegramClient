using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1614803355)]
    public class TlInputMessagesFilterVideo : TlAbsMessagesFilter
    {
        public override int Constructor => -1614803355;


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