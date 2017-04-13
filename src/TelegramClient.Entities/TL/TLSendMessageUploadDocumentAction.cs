using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1441998364)]
    public class TlSendMessageUploadDocumentAction : TlAbsSendMessageAction
    {
        public override int Constructor => -1441998364;

        public int Progress { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Progress = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Progress);
        }
    }
}