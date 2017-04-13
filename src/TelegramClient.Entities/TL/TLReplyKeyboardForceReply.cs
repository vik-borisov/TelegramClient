using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-200242528)]
    public class TlReplyKeyboardForceReply : TlAbsReplyMarkup
    {
        public override int Constructor => -200242528;

        public int Flags { get; set; }
        public bool SingleUse { get; set; }
        public bool Selective { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = SingleUse ? Flags | 2 : Flags & ~2;
            Flags = Selective ? Flags | 4 : Flags & ~4;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            SingleUse = (Flags & 2) != 0;
            Selective = (Flags & 4) != 0;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
        }
    }
}