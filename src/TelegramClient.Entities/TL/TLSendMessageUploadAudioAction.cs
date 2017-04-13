using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-212740181)]
    public class TlSendMessageUploadAudioAction : TlAbsSendMessageAction
    {
        public override int Constructor => -212740181;

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