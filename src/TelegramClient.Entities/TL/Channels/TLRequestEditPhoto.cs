using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-248621111)]
    public class TlRequestEditPhoto : TlMethod
    {
        public override int Constructor => -248621111;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsInputChatPhoto Photo { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Photo = (TlAbsInputChatPhoto) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(Photo, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}