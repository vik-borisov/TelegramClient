using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(9203775)]
    public class TlUserStatusOffline : TlAbsUserStatus
    {
        public override int Constructor => 9203775;

        public int WasOnline { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            WasOnline = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(WasOnline);
        }
    }
}