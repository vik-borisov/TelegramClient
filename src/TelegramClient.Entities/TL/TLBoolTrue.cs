using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1720552011)]
    public class TlBoolTrue : TlAbsBool
    {
        public override int Constructor => -1720552011;


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