using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-1663104819)]
    public class TlRequestGetSupport : TlMethod
    {
        public override int Constructor => -1663104819;

        public TlSupport Response { get; set; }


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
            Response = (TlSupport) ObjectUtils.DeserializeObject(br);
        }
    }
}