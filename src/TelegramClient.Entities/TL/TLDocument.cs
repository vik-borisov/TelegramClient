using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-2027738169)]
    public class TlDocument : TlAbsDocument
    {
        public override int Constructor => -2027738169;

        public long Id { get; set; }
        public long AccessHash { get; set; }
        public int Date { get; set; }
        public string MimeType { get; set; }
        public int Size { get; set; }
        public TlAbsPhotoSize Thumb { get; set; }
        public int DcId { get; set; }
        public int Version { get; set; }
        public TlVector<TlAbsDocumentAttribute> Attributes { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
            Date = br.ReadInt32();
            MimeType = StringUtil.Deserialize(br);
            Size = br.ReadInt32();
            Thumb = (TlAbsPhotoSize) ObjectUtils.DeserializeObject(br);
            DcId = br.ReadInt32();
            Version = br.ReadInt32();
            Attributes = ObjectUtils.DeserializeVector<TlAbsDocumentAttribute>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(AccessHash);
            bw.Write(Date);
            StringUtil.Serialize(MimeType, bw);
            bw.Write(Size);
            ObjectUtils.SerializeObject(Thumb, bw);
            bw.Write(DcId);
            bw.Write(Version);
            ObjectUtils.SerializeObject(Attributes, bw);
        }
    }
}