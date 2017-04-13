using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1782549861)]
    public class TlRequestGetAllDrafts : TlMethod
    {
        public override int Constructor => 1782549861;

        public TlAbsUpdates Response { get; set; }


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
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}