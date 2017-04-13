using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1122524854)]
    public class TlTopPeerCategoryGroups : TlAbsTopPeerCategory
    {
        public override int Constructor => -1122524854;


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