using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-878758099)]
    public class TlRequestInvokeAfterMsg : TlMethod
    {
        public override int Constructor => -878758099;

        public long MsgId { get; set; }
        public TlObject Query { get; set; }
        public TlObject Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            MsgId = br.ReadInt64();
            Query = (TlObject) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(MsgId);
            ObjectUtils.SerializeObject(Query, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlObject) ObjectUtils.DeserializeObject(br);
        }
    }
}