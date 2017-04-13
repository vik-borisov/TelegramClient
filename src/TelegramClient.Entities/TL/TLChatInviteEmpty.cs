using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1776236393)]
    public class TlChatInviteEmpty : TlAbsExportedChatInvite
    {
        public override int Constructor => 1776236393;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}