using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(457133559)]
    public class TlUpdateEditChannelMessage : TlAbsUpdate
    {
        public override int Constructor => 457133559;

        public TlAbsMessage Message { get; set; }
        public int Pts { get; set; }
        public int PtsCount { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Message = (TlAbsMessage) ObjectUtils.DeserializeObject(br);
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Message, bw);
            bw.Write(Pts);
            bw.Write(PtsCount);
        }
    }
}