using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-247351839)]
    public class TlInputEncryptedChat : TlObject
    {
        public override int Constructor => -247351839;

        public int ChatId { get; set; }
        public long AccessHash { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChatId = br.ReadInt32();
            AccessHash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChatId);
            bw.Write(AccessHash);
        }
    }
}