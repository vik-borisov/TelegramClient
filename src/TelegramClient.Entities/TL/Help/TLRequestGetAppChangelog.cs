using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-1189013126)]
    public class TlRequestGetAppChangelog : TlMethod
    {
        public override int Constructor => -1189013126;

        public TlAbsAppChangelog Response { get; set; }


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

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsAppChangelog) ObjectUtils.DeserializeObject(br);
        }
    }
}