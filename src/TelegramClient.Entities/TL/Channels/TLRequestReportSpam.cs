using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-32999408)]
    public class TlRequestReportSpam : TlMethod
    {
        public override int Constructor => -32999408;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlVector<int> Id { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Id = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(UserId, bw);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}