using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1662091044)]
    public class TlWallPaperSolid : TlAbsWallPaper
    {
        public override int Constructor => 1662091044;

        public int Id { get; set; }
        public string Title { get; set; }
        public int BgColor { get; set; }
        public int Color { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            Title = StringUtil.Deserialize(br);
            BgColor = br.ReadInt32();
            Color = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            StringUtil.Serialize(Title, bw);
            bw.Write(BgColor);
            bw.Write(Color);
        }
    }
}