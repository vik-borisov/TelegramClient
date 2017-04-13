using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1035731989)]
    public class TlRequestAcceptEncryption : TlMethod
    {
        public override int Constructor => 1035731989;

        public TlInputEncryptedChat Peer { get; set; }
        public byte[] GB { get; set; }
        public long KeyFingerprint { get; set; }
        public TlAbsEncryptedChat Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlInputEncryptedChat) ObjectUtils.DeserializeObject(br);
            GB = BytesUtil.Deserialize(br);
            KeyFingerprint = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            BytesUtil.Serialize(GB, bw);
            bw.Write(KeyFingerprint);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsEncryptedChat) ObjectUtils.DeserializeObject(br);
        }
    }
}