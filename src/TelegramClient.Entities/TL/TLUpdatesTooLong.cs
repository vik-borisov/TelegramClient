using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-484987010)]
    public class TlUpdatesTooLong : TlAbsUpdates
    {
        public override int Constructor => -484987010;


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