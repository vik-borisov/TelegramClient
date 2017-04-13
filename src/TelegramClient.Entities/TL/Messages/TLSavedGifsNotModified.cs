using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-402498398)]
    public class TlSavedGifsNotModified : TlAbsSavedGifs
    {
        public override int Constructor => -402498398;


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