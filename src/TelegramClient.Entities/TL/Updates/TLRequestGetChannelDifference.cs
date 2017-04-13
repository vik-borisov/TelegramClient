using System.IO;

namespace TelegramClient.Entities.TL.Updates
{
    [TlObject(-1154295872)]
    public class TlRequestGetChannelDifference : TlMethod
    {
        public override int Constructor => -1154295872;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsChannelMessagesFilter Filter { get; set; }
        public int Pts { get; set; }
        public int Limit { get; set; }
        public TlAbsChannelDifference Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Filter = (TlAbsChannelMessagesFilter) ObjectUtils.DeserializeObject(br);
            Pts = br.ReadInt32();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(Filter, bw);
            bw.Write(Pts);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsChannelDifference) ObjectUtils.DeserializeObject(br);
        }
    }
}