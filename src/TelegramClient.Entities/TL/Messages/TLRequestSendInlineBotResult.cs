using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1318189314)]
    public class TlRequestSendInlineBotResult : TlMethod
    {
        public override int Constructor => -1318189314;

        public int Flags { get; set; }
        public bool Silent { get; set; }
        public bool Background { get; set; }
        public bool ClearDraft { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public int? ReplyToMsgId { get; set; }
        public long RandomId { get; set; }
        public long QueryId { get; set; }
        public string Id { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Silent ? Flags | 32 : Flags & ~32;
            Flags = Background ? Flags | 64 : Flags & ~64;
            Flags = ClearDraft ? Flags | 128 : Flags & ~128;
            Flags = ReplyToMsgId != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Silent = (Flags & 32) != 0;
            Background = (Flags & 64) != 0;
            ClearDraft = (Flags & 128) != 0;
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                ReplyToMsgId = br.ReadInt32();
            else
                ReplyToMsgId = null;

            RandomId = br.ReadInt64();
            QueryId = br.ReadInt64();
            Id = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            ObjectUtils.SerializeObject(Peer, bw);
            if ((Flags & 1) != 0)
                bw.Write(ReplyToMsgId.Value);
            bw.Write(RandomId);
            bw.Write(QueryId);
            StringUtil.Serialize(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}