using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-990308245)]
    public class TlRequestGetConfig : TlMethod
    {
        public override int Constructor => -990308245;

        public TlConfig Response { get; set; }


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
            Response = (TlConfig) ObjectUtils.DeserializeObject(br);
        }
    }
}