using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-2130010132)]
    public class TlRequestGetBotCallbackAnswer : TlMethod
    {
        public override int Constructor => -2130010132;

        public int Flags { get; set; }
        public bool Game { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public int MsgId { get; set; }
        public byte[] Data { get; set; }
        public TlBotCallbackAnswer Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Game ? Flags | 2 : Flags & ~2;
            Flags = Data != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Game = (Flags & 2) != 0;
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            MsgId = br.ReadInt32();
            if ((Flags & 1) != 0)
                Data = BytesUtil.Deserialize(br);
            else
                Data = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MsgId);
            if ((Flags & 1) != 0)
                BytesUtil.Serialize(Data, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlBotCallbackAnswer) ObjectUtils.DeserializeObject(br);
        }
    }
}