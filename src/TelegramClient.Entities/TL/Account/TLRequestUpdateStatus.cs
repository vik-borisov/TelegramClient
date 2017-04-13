using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(1713919532)]
    public class TlRequestUpdateStatus : TlMethod
    {
        public override int Constructor => 1713919532;

        public bool Offline { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Offline = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BoolUtil.Serialize(Offline, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}