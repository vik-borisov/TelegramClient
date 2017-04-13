using System.IO;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-1814580409)]
    public class TlRequestGetMessages : TlMethod
    {
        public override int Constructor => -1814580409;

        public TlAbsInputChannel Channel { get; set; }
        public TlVector<int> Id { get; set; }
        public TlAbsMessages Response { get; set; }


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
            Response = (TlAbsMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}