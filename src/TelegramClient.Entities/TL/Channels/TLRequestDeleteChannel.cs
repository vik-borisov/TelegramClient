using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-1072619549)]
    public class TlRequestDeleteChannel : TlMethod
    {
        public override int Constructor => -1072619549;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}