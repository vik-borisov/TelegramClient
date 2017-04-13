using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-470837741)]
    public class TlRequestImportAuthorization : TlMethod
    {
        public override int Constructor => -470837741;

        public int Id { get; set; }
        public byte[] Bytes { get; set; }
        public TlAuthorization Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            Bytes = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            BytesUtil.Serialize(Bytes, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAuthorization) ObjectUtils.DeserializeObject(br);
        }
    }
}