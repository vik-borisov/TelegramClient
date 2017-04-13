using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(649453030)]
    public class TlMessageEditData : TlObject
    {
        public override int Constructor => 649453030;

        public int Flags { get; set; }
        public bool Caption { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Caption ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Caption = (Flags & 1) != 0;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
        }
    }
}