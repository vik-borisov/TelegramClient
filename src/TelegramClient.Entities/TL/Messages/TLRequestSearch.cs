using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-732523960)]
    public class TlRequestSearch : TlMethod
    {
        public override int Constructor => -732523960;

        public int Flags { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public string Q { get; set; }
        public TlAbsMessagesFilter Filter { get; set; }
        public int MinDate { get; set; }
        public int MaxDate { get; set; }
        public int Offset { get; set; }
        public int MaxId { get; set; }
        public int Limit { get; set; }
        public TlAbsMessages Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            Q = StringUtil.Deserialize(br);
            Filter = (TlAbsMessagesFilter) ObjectUtils.DeserializeObject(br);
            MinDate = br.ReadInt32();
            MaxDate = br.ReadInt32();
            Offset = br.ReadInt32();
            MaxId = br.ReadInt32();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
            StringUtil.Serialize(Q, bw);
            ObjectUtils.SerializeObject(Filter, bw);
            bw.Write(MinDate);
            bw.Write(MaxDate);
            bw.Write(Offset);
            bw.Write(MaxId);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}