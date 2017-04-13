using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(2016638777)]
    public class TlRequestReorderStickerSets : TlMethod
    {
        public override int Constructor => 2016638777;

        public int Flags { get; set; }
        public bool Masks { get; set; }
        public TlVector<long> Order { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Masks ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Masks = (Flags & 1) != 0;
            Order = ObjectUtils.DeserializeVector<long>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            ObjectUtils.SerializeObject(Order, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}