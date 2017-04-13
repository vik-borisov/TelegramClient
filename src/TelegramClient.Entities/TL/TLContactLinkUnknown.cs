using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1599050311)]
    public class TlContactLinkUnknown : TlAbsContactLink
    {
        public override int Constructor => 1599050311;


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