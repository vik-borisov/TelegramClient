using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(371037736)]
    public class TlTopPeerCategoryChannels : TlAbsTopPeerCategory
    {
        public override int Constructor => 371037736;


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