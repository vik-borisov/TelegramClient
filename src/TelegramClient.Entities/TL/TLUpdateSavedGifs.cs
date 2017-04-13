using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1821035490)]
    public class TlUpdateSavedGifs : TlAbsUpdate
    {
        public override int Constructor => -1821035490;


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