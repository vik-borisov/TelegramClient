using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1107729093)]
    public class TlGame : TlObject
    {
        public override int Constructor => -1107729093;

        public int Flags { get; set; }
        public long Id { get; set; }
        public long AccessHash { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TlAbsPhoto Photo { get; set; }
        public TlAbsDocument Document { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Document != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
            ShortName = StringUtil.Deserialize(br);
            Title = StringUtil.Deserialize(br);
            Description = StringUtil.Deserialize(br);
            Photo = (TlAbsPhoto) ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                Document = (TlAbsDocument) ObjectUtils.DeserializeObject(br);
            else
                Document = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Id);
            bw.Write(AccessHash);
            StringUtil.Serialize(ShortName, bw);
            StringUtil.Serialize(Title, bw);
            StringUtil.Serialize(Description, bw);
            ObjectUtils.SerializeObject(Photo, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(Document, bw);
        }
    }
}