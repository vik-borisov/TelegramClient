using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(1418342645)]
    public class TlRequestGetPassword : TlMethod
    {
        public override int Constructor => 1418342645;

        public TlAbsPassword Response { get; set; }


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
            Response = (TlAbsPassword) ObjectUtils.DeserializeObject(br);
        }
    }
}