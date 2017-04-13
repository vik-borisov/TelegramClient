using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1358283666)]
    public class TlInputMessagesFilterVoice : TlAbsMessagesFilter
    {
        public override int Constructor => 1358283666;


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