using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(-304838614)]
    public class TlRequestGetState : TlMethod
    {
        public override int Constructor => -304838614;

        public TlState Response { get; set; }


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
            Response = (TlState) ObjectUtils.DeserializeObject(br);
        }
    }
}