using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1799878989)]
    public class TlRequestGetDialogs : TlMethod
    {
        public override int Constructor => 1799878989;

        public int OffsetDate { get; set; }
        public int OffsetId { get; set; }
        public TlAbsInputPeer OffsetPeer { get; set; }
        public int Limit { get; set; }
        public TlAbsDialogs Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            OffsetDate = br.ReadInt32();
            OffsetId = br.ReadInt32();
            OffsetPeer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(OffsetDate);
            bw.Write(OffsetId);
            ObjectUtils.SerializeObject(OffsetPeer, bw);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsDialogs) ObjectUtils.DeserializeObject(br);
        }
    }
}