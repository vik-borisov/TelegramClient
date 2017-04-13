using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(344356834)]
    public class TlTopPeerCategoryBotsInline : TlAbsTopPeerCategory
    {
        public override int Constructor => 344356834;


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