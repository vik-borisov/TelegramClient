using System.IO;

namespace TelegramClient.Entities.TL
{
    [TLObject(-138301121)]
    public class TLInputUserSelf : TLAbsInputUser
    {
        public override int Constructor => -138301121;


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