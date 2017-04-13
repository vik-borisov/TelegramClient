using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(1231065863)]
    public class TlRequestToggleInvites : TlMethod
    {
        public override int Constructor => 1231065863;

        public TlAbsInputChannel Channel { get; set; }
        public bool Enabled { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Enabled = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            BoolUtil.Serialize(Enabled, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}