using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-627372787)]
    public class TlRequestInvokeWithLayer : TlMethod
    {
        public override int Constructor => -627372787;

        public int Layer { get; set; }
        public TlObject Query { get; set; }
        public TlObject Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Layer = br.ReadInt32();
            Query = (TlObject) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Layer);
            ObjectUtils.SerializeObject(Query, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlObject) ObjectUtils.DeserializeObject(br);
        }
    }
}