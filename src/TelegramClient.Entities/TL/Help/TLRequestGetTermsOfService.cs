using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(889286899)]
    public class TlRequestGetTermsOfService : TlMethod
    {
        public override int Constructor => 889286899;

        public TlTermsOfService Response { get; set; }


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
            Response = (TlTermsOfService) ObjectUtils.DeserializeObject(br);
        }
    }
}