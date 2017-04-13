using System.IO;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-787622117)]
    public class TlRequestDeleteUserHistory : TlMethod
    {
        public override int Constructor => -787622117;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsInputUser UserId { get; set; }
        public TlAffectedHistory Response { get; set; }


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
            Response = (TlAffectedHistory) ObjectUtils.DeserializeObject(br);
        }
    }
}