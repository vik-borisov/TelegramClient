using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-306628279)]
    public class TlUserStatusOnline : TlAbsUserStatus
    {
        public override int Constructor => -306628279;

        public int Expires { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Expires = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Expires);
        }
    }
}