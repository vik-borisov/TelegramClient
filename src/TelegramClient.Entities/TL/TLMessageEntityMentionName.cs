using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(892193368)]
    public class TlMessageEntityMentionName : TlAbsMessageEntity
    {
        public override int Constructor => 892193368;

        public int Offset { get; set; }
        public int Length { get; set; }
        public int UserId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Offset = br.ReadInt32();
            Length = br.ReadInt32();
            UserId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Offset);
            bw.Write(Length);
            bw.Write(UserId);
        }
    }
}