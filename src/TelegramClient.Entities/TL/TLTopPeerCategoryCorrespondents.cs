using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(104314861)]
    public class TlTopPeerCategoryCorrespondents : TlAbsTopPeerCategory
    {
        public override int Constructor => 104314861;


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