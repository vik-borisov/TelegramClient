using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1557277184)]
    public class TlMessageMediaWebPage : TlAbsMessageMedia
    {
        public override int Constructor => -1557277184;

        public TlAbsWebPage Webpage { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Webpage = (TlAbsWebPage) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Webpage, bw);
        }
    }
}