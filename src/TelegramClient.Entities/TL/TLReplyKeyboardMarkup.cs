using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(889353612)]
    public class TlReplyKeyboardMarkup : TlAbsReplyMarkup
    {
        public override int Constructor => 889353612;

        public int Flags { get; set; }
        public bool Resize { get; set; }
        public bool SingleUse { get; set; }
        public bool Selective { get; set; }
        public TlVector<TlKeyboardButtonRow> Rows { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Resize ? Flags | 1 : Flags & ~1;
            Flags = SingleUse ? Flags | 2 : Flags & ~2;
            Flags = Selective ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Resize = (Flags & 1) != 0;
            SingleUse = (Flags & 2) != 0;
            Selective = (Flags & 4) != 0;
            Rows = ObjectUtils.DeserializeVector<TlKeyboardButtonRow>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            ObjectUtils.SerializeObject(Rows, bw);
        }
    }
}