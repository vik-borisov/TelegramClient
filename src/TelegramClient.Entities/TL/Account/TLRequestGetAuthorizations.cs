using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-484392616)]
    public class TlRequestGetAuthorizations : TlMethod
    {
        public override int Constructor => -484392616;

        public TlAuthorizations Response { get; set; }


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
            Response = (TlAuthorizations) ObjectUtils.DeserializeObject(br);
        }
    }
}