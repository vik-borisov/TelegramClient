using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1728035348)]
    public class TlDialog : TlObject
    {
        public override int Constructor => 1728035348;

        public int Flags { get; set; }
        public TlAbsPeer Peer { get; set; }
        public int TopMessage { get; set; }
        public int ReadInboxMaxId { get; set; }
        public int ReadOutboxMaxId { get; set; }
        public int UnreadCount { get; set; }
        public TlAbsPeerNotifySettings NotifySettings { get; set; }
        public int? Pts { get; set; }
        public TlAbsDraftMessage Draft { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Pts != null ? Flags | 1 : Flags & ~1;
            Flags = Draft != null ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Peer = (TlAbsPeer) ObjectUtils.DeserializeObject(br);
            TopMessage = br.ReadInt32();
            ReadInboxMaxId = br.ReadInt32();
            ReadOutboxMaxId = br.ReadInt32();
            UnreadCount = br.ReadInt32();
            NotifySettings = (TlAbsPeerNotifySettings) ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                Pts = br.ReadInt32();
            else
                Pts = null;

            if ((Flags & 2) != 0)
                Draft = (TlAbsDraftMessage) ObjectUtils.DeserializeObject(br);
            else
                Draft = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(TopMessage);
            bw.Write(ReadInboxMaxId);
            bw.Write(ReadOutboxMaxId);
            bw.Write(UnreadCount);
            ObjectUtils.SerializeObject(NotifySettings, bw);
            if ((Flags & 1) != 0)
                bw.Write(Pts.Value);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(Draft, bw);
        }
    }
}