using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-378127636)]
    public class TlSendMessageUploadVideoAction : TlAbsSendMessageAction
    {
        public override int Constructor => -378127636;

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