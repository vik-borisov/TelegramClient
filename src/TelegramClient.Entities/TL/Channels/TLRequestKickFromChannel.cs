using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-1502421484)]
    public class TlRequestKickFromChannel : TlMethod
    {
        public override int Constructor => -1502421484;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public bool Kicked { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Kicked = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(UserId, bw);
            BoolUtil.Serialize(Kicked, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}