using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(546203849)]
    public class TlInputMessageEntityMentionName : TlAbsMessageEntity
    {
        public override int Constructor => 546203849;

        public int Offset { get; set; }
        public int Length { get; set; }
        public TlAbsInputUser UserId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Offset = br.ReadInt32();
            Length = br.ReadInt32();
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Offset);
            bw.Write(Length);
            ObjectUtils.SerializeObject(UserId, bw);
        }
    }
}