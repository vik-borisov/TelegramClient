using System.IO;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(176122811)]
    public class TlRequestGetChannels : TlMethod
    {
        public override int Constructor => 176122811;

        public TlVector<TlAbsInputChannel> Id { get; set; }
        public TlChats Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = ObjectUtils.DeserializeVector<TlAbsInputChannel>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlChats) ObjectUtils.DeserializeObject(br);
        }
    }
}