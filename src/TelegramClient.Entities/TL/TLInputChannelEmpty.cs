using System.IO;

namespace TelegramClient.Entities.TL
{
    [TLObject(-292807034)]
    public class TLInputChannelEmpty : TLAbsInputChannel
    {
        public override int Constructor => -292807034;


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