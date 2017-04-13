using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-871347913)]
    public class TlRequestReadHistory : TlMethod
    {
        public override int Constructor => -871347913;

        public TlAbsInputChannel Channel { get; set; }
        public int MaxId { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            MaxId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            bw.Write(MaxId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}