using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(1461180992)]
    public class TlRequestLogOut : TlMethod
    {
        public override int Constructor => 1461180992;

        public bool Response { get; set; }


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
            Response = BoolUtil.Deserialize(br);
        }
    }
}