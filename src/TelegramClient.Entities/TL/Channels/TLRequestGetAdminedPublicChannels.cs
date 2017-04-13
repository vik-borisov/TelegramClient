using System.IO;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-1920105769)]
    public class TlRequestGetAdminedPublicChannels : TlMethod
    {
        public override int Constructor => -1920105769;

        public TlChats Response { get; set; }


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

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlChats) ObjectUtils.DeserializeObject(br);
        }
    }
}