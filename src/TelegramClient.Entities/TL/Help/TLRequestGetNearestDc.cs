using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(531836966)]
    public class TlRequestGetNearestDc : TlMethod
    {
        public override int Constructor => 531836966;

        public TlNearestDc Response { get; set; }


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
            Response = (TlNearestDc) ObjectUtils.DeserializeObject(br);
        }
    }
}