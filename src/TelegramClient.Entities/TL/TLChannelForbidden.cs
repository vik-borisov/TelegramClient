using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-2059962289)]
    public class TlChannelForbidden : TlAbsChat
    {
        public override int Constructor => -2059962289;

        public int Flags { get; set; }
        public bool Broadcast { get; set; }
        public bool Megagroup { get; set; }
        public int Id { get; set; }
        public long AccessHash { get; set; }
        public string Title { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Broadcast ? Flags | 32 : Flags & ~32;
            Flags = Megagroup ? Flags | 256 : Flags & ~256;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Broadcast = (Flags & 32) != 0;
            Megagroup = (Flags & 256) != 0;
            Id = br.ReadInt32();
            AccessHash = br.ReadInt64();
            Title = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            bw.Write(Id);
            bw.Write(AccessHash);
            StringUtil.Serialize(Title, bw);
        }
    }
}