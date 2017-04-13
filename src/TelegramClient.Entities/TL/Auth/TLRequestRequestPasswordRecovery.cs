using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-661144474)]
    public class TlRequestRequestPasswordRecovery : TlMethod
    {
        public override int Constructor => -661144474;

        public TlPasswordRecovery Response { get; set; }


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
            Response = (TlPasswordRecovery) ObjectUtils.DeserializeObject(br);
        }
    }
}