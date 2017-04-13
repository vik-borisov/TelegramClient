using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1347868602)]
    public class TlRequestGetHistory : TlMethod
    {
        public override int Constructor => -1347868602;

        public TlAbsInputPeer Peer { get; set; }
        public int OffsetId { get; set; }
        public int OffsetDate { get; set; }
        public int AddOffset { get; set; }
        public int Limit { get; set; }
        public int MaxId { get; set; }
        public int MinId { get; set; }
        public TlAbsMessages Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            OffsetId = br.ReadInt32();
            OffsetDate = br.ReadInt32();
            AddOffset = br.ReadInt32();
            Limit = br.ReadInt32();
            MaxId = br.ReadInt32();
            MinId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(OffsetId);
            bw.Write(OffsetDate);
            bw.Write(AddOffset);
            bw.Write(Limit);
            bw.Write(MaxId);
            bw.Write(MinId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}