using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-415938591)]
    public class TlUpdateBotCallbackQuery : TlAbsUpdate
    {
        public override int Constructor => -415938591;

        public int Flags { get; set; }
        public long QueryId { get; set; }
        public int UserId { get; set; }
        public TlAbsPeer Peer { get; set; }
        public int MsgId { get; set; }
        public long ChatInstance { get; set; }
        public byte[] Data { get; set; }
        public string GameShortName { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Data != null ? Flags | 1 : Flags & ~1;
            Flags = GameShortName != null ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            QueryId = br.ReadInt64();
            UserId = br.ReadInt32();
            Peer = (TlAbsPeer) ObjectUtils.DeserializeObject(br);
            MsgId = br.ReadInt32();
            ChatInstance = br.ReadInt64();
            if ((Flags & 1) != 0)
                Data = BytesUtil.Deserialize(br);
            else
                Data = null;

            if ((Flags & 2) != 0)
                GameShortName = StringUtil.Deserialize(br);
            else
                GameShortName = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(QueryId);
            bw.Write(UserId);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MsgId);
            bw.Write(ChatInstance);
            if ((Flags & 1) != 0)
                BytesUtil.Serialize(Data, bw);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(GameShortName, bw);
        }
    }
}