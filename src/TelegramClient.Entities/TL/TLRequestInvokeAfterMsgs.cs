using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1036301552)]
    public class TlRequestInvokeAfterMsgs : TlMethod
    {
        public override int Constructor => 1036301552;

        public TlVector<long> MsgIds { get; set; }
        public TlObject Query { get; set; }
        public TlObject Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            MsgIds = ObjectUtils.DeserializeVector<long>(br);
            Query = (TlObject) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(MsgIds, bw);
            ObjectUtils.SerializeObject(Query, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlObject) ObjectUtils.DeserializeObject(br);
        }
    }
}