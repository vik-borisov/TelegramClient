using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(1416484774)]
    public class TlRequestGetParticipant : TlMethod
    {
        public override int Constructor => 1416484774;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlChannelParticipant Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(UserId, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlChannelParticipant) ObjectUtils.DeserializeObject(br);
        }
    }
}