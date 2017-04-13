using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1132882121)]
    public class TlBoolFalse : TlAbsBool
    {
        public override int Constructor => -1132882121;


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