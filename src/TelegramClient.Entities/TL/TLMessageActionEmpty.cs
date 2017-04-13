using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1230047312)]
    public class TlMessageActionEmpty : TlAbsMessageAction
    {
        public override int Constructor => -1230047312;


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