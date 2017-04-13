using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-38694904)]
    public class TlMessageMediaGame : TlAbsMessageMedia
    {
        public override int Constructor => -38694904;

        public TlGame Game { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Game = (TlGame) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Game, bw);
        }
    }
}