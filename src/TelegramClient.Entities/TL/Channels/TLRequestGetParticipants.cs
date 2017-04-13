using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(618237842)]
    public class TlRequestGetParticipants : TlMethod
    {
        public override int Constructor => 618237842;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsChannelParticipantsFilter Filter { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public TlChannelParticipants Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            Filter = (TlAbsChannelParticipantsFilter) ObjectUtils.DeserializeObject(br);
            Offset = br.ReadInt32();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(Filter, bw);
            bw.Write(Offset);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlChannelParticipants) ObjectUtils.DeserializeObject(br);
        }
    }
}