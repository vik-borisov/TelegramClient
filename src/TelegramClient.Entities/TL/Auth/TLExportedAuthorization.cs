using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-543777747)]
    public class TlExportedAuthorization : TlObject
    {
        public override int Constructor => -543777747;

        public int Id { get; set; }
        public byte[] Bytes { get; set; }


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
    }
}