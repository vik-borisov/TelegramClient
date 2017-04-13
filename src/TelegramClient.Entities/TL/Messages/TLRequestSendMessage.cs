using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-91733382)]
    public class TlRequestSendMessage : TlMethod
    {
        public override int Constructor => -91733382;

        public int Flags { get; set; }
        public bool NoWebpage { get; set; }
        public bool Silent { get; set; }
        public bool Background { get; set; }
        public bool ClearDraft { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public int? ReplyToMsgId { get; set; }
        public string Message { get; set; }
        public long RandomId { get; set; }
        public TlAbsReplyMarkup ReplyMarkup { get; set; }
        public TlVector<TlAbsMessageEntity> Entities { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = NoWebpage ? Flags | 2 : Flags & ~2;
            Flags = Silent ? Flags | 32 : Flags & ~32;
            Flags = Background ? Flags | 64 : Flags & ~64;
            Flags = ClearDraft ? Flags | 128 : Flags & ~128;
            Flags = ReplyToMsgId != null ? Flags | 1 : Flags & ~1;
            Flags = ReplyMarkup != null ? Flags | 4 : Flags & ~4;
            Flags = Entities != null ? Flags | 8 : Flags & ~8;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            NoWebpage = (Flags & 2) != 0;
            Silent = (Flags & 32) != 0;
            Background = (Flags & 64) != 0;
            ClearDraft = (Flags & 128) != 0;
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                ReplyToMsgId = br.ReadInt32();
            else
                ReplyToMsgId = null;

            Message = StringUtil.Deserialize(br);
            RandomId = br.ReadInt64();
            if ((Flags & 4) != 0)
                ReplyMarkup = (TlAbsReplyMarkup) ObjectUtils.DeserializeObject(br);
            else
                ReplyMarkup = null;

            if ((Flags & 8) != 0)
                Entities = ObjectUtils.DeserializeVector<TlAbsMessageEntity>(br);
            else
                Entities = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            ObjectUtils.SerializeObject(Peer, bw);
            if ((Flags & 1) != 0)
                bw.Write(ReplyToMsgId.Value);
            StringUtil.Serialize(Message, bw);
            bw.Write(RandomId);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
            if ((Flags & 8) != 0)
                ObjectUtils.SerializeObject(Entities, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}