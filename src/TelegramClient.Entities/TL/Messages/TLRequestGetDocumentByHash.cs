using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(864953444)]
    public class TlRequestGetDocumentByHash : TlMethod
    {
        public override int Constructor => 864953444;

        public byte[] Sha256 { get; set; }
        public int Size { get; set; }
        public string MimeType { get; set; }
        public TlAbsDocument Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Sha256 = BytesUtil.Deserialize(br);
            Size = br.ReadInt32();
            MimeType = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BytesUtil.Serialize(Sha256, bw);
            bw.Write(Size);
            StringUtil.Serialize(MimeType, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsDocument) ObjectUtils.DeserializeObject(br);
        }
    }
}