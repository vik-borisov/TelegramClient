using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1798033689)]
    public class TlChannelMessagesFilterEmpty : TlAbsChannelMessagesFilter
    {
        public override int Constructor => -1798033689;


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