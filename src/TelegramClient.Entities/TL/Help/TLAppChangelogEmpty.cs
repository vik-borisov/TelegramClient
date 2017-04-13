using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-1350696044)]
    public class TlAppChangelogEmpty : TlAbsAppChangelog
    {
        public override int Constructor => -1350696044;


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