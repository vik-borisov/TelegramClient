using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-567906571)]
    public class TlTopPeersNotModified : TlAbsTopPeers
    {
        public override int Constructor => -567906571;


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