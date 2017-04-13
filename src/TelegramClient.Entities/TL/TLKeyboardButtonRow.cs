using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(2002815875)]
    public class TlKeyboardButtonRow : TlObject
    {
        public override int Constructor => 2002815875;

        public TlVector<TlAbsKeyboardButton> Buttons { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Buttons = ObjectUtils.DeserializeVector<TlAbsKeyboardButton>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Buttons, bw);
        }
    }
}