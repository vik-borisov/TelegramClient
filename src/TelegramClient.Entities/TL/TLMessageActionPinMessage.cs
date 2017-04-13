using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1799538451)]
    public class TlMessageActionPinMessage : TlAbsMessageAction
    {
        public override int Constructor => -1799538451;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}