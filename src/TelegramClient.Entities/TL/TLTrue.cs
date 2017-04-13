using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1072550713)]
    public class TlTrue : TlObject
    {
        public override int Constructor => 1072550713;


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