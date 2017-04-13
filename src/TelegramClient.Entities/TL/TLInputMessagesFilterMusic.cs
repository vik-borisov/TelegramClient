using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(928101534)]
    public class TlInputMessagesFilterMusic : TlAbsMessagesFilter
    {
        public override int Constructor => 928101534;


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