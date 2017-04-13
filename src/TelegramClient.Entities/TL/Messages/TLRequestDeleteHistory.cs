using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(469850889)]
    public class TlRequestDeleteHistory : TlMethod
    {
        public override int Constructor => 469850889;

        public int Flags { get; set; }
        public bool JustClear { get; set; }
        public TlAbsInputPeer Peer { get; set; }
        public int MaxId { get; set; }
        public TlAffectedHistory Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = JustClear ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            JustClear = (Flags & 1) != 0;
            Peer = (TlAbsInputPeer) ObjectUtils.DeserializeObject(br);
            MaxId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MaxId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAffectedHistory) ObjectUtils.DeserializeObject(br);
        }
    }
}