using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(2135648522)]
    public class TlRequestReadEncryptedHistory : TlMethod
    {
        public override int Constructor => 2135648522;

        public TlInputEncryptedChat Peer { get; set; }
        public int MaxDate { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TlInputEncryptedChat) ObjectUtils.DeserializeObject(br);
            MaxDate = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MaxDate);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}