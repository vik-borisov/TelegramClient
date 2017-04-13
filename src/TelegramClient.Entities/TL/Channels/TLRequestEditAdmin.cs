using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-344583728)]
    public class TlRequestEditAdmin : TlMethod
    {
        public override int Constructor => -344583728;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlAbsChannelParticipantRole Role { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            Role = (TlAbsChannelParticipantRole) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(UserId, bw);
            ObjectUtils.SerializeObject(Role, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}