using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-162681021)]
    public class TlRequestRequestEncryption : TlMethod
    {
        public override int Constructor => -162681021;

        public TlAbsInputUser UserId { get; set; }
        public int RandomId { get; set; }
        public byte[] GA { get; set; }
        public TlAbsEncryptedChat Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = (TlAbsInputUser) ObjectUtils.DeserializeObject(br);
            RandomId = br.ReadInt32();
            GA = BytesUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(UserId, bw);
            bw.Write(RandomId);
            BytesUtil.Serialize(GA, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsEncryptedChat) ObjectUtils.DeserializeObject(br);
        }
    }
}