using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1475442322)]
    public class TlRequestGetArchivedStickers : TlMethod
    {
        public override int Constructor => 1475442322;

        public int Flags { get; set; }
        public bool Masks { get; set; }
        public long OffsetId { get; set; }
        public int Limit { get; set; }
        public TlArchivedStickers Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Masks ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Masks = (Flags & 1) != 0;
            OffsetId = br.ReadInt64();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            bw.Write(OffsetId);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlArchivedStickers) ObjectUtils.DeserializeObject(br);
        }
    }
}