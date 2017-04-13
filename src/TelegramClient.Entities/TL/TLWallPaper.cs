using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-860866985)]
    public class TlWallPaper : TlAbsWallPaper
    {
        public override int Constructor => -860866985;

        public int Id { get; set; }
        public string Title { get; set; }
        public TlVector<TlAbsPhotoSize> Sizes { get; set; }
        public int Color { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            Title = StringUtil.Deserialize(br);
            Sizes = ObjectUtils.DeserializeVector<TlAbsPhotoSize>(br);
            Color = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            StringUtil.Serialize(Title, bw);
            ObjectUtils.SerializeObject(Sizes, bw);
            bw.Write(Color);
        }
    }
}