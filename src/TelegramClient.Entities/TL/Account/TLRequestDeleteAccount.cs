using System.IO;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(1099779595)]
    public class TlRequestDeleteAccount : TlMethod
    {
        public override int Constructor => 1099779595;

        public string Reason { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Reason = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Reason, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}