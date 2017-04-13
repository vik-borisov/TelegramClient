using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1080796745)]
    public class TlRequestInvokeWithoutUpdates : TlMethod
    {
        public override int Constructor => -1080796745;

        public TlObject Query { get; set; }
        public TlObject Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Query = (TlObject) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Query, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlObject) ObjectUtils.DeserializeObject(br);
        }
    }
}