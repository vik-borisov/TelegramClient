using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-292807034)]
    public class TlInputChannelEmpty : TlAbsInputChannel
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