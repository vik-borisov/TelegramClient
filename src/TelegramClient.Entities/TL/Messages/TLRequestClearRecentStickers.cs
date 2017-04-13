using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1986437075)]
    public class TlRequestClearRecentStickers : TlMethod
    {
        public override int Constructor => -1986437075;

        public int Flags { get; set; }
        public bool Attached { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Attached ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Attached = (Flags & 1) != 0;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}