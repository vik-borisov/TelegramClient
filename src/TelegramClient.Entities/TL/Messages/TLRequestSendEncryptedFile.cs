using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1701831834)]
    public class TlRequestSendEncryptedFile : TlMethod
    {
        public override int Constructor => -1701831834;

        public TlInputEncryptedChat Peer { get; set; }
        public long RandomId { get; set; }
        public byte[] Data { get; set; }
        public TlAbsInputEncryptedFile File { get; set; }
        public TlAbsSentEncryptedMessage Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlInputEncryptedChat) ObjectUtils.DeserializeObject(br);
            RandomId = br.ReadInt64();
            Data = BytesUtil.Deserialize(br);
            File = (TlAbsInputEncryptedFile) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(RandomId);
            BytesUtil.Serialize(Data, bw);
            ObjectUtils.SerializeObject(File, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsSentEncryptedMessage) ObjectUtils.DeserializeObject(br);
        }
    }
}