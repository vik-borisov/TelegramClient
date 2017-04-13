using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1371385889)]
    public class TlMessageActionChatMigrateTo : TlAbsMessageAction
    {
        public override int Constructor => 1371385889;

        public int ChannelId { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
        }
    }
}