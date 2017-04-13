using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(429865580)]
    public class TlRequestInviteToChannel : TlMethod
    {
        public override int Constructor => 429865580;

        public TlAbsInputChannel Channel { get; set; }
        public TlVector<TlAbsInputUser> Users { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Users = ObjectUtils.DeserializeVector<TlAbsInputUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}