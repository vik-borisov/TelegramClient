using System.IO;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-2067661490)]
    public class TlRequestDeleteMessages : TlMethod
    {
        public override int Constructor => -2067661490;

        public TlAbsInputChannel Channel { get; set; }
        public TlVector<int> Id { get; set; }
        public TlAffectedMessages Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Id = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAffectedMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}