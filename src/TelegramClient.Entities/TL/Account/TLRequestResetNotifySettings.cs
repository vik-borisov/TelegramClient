using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(-612493497)]
    public class TlRequestResetNotifySettings : TlMethod
    {
        public override int Constructor => -612493497;

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