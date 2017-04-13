using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(1295590211)]
    public class TlRequestGetInviteText : TlMethod
    {
        public override int Constructor => 1295590211;

        public TlInviteText Response { get; set; }


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
            Response = (TlInviteText) ObjectUtils.DeserializeObject(br);
        }
    }
}