using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1527873830)]
    public class TlRequestReadFeaturedStickers : TlMethod
    {
        public override int Constructor => 1527873830;

        public TlVector<long> Id { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = ObjectUtils.DeserializeVector<long>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}