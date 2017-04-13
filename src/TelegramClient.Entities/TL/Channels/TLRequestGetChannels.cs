using System.IO;
using TelegramClient.Entities.TL.Messages;

namespace TelegramClient.Entities.TL.Channels
{
    [TLObject(176122811)]
    public class TLRequestGetChannels : TLMethod
    {
        public override int Constructor => 176122811;

        public TLVector<TLAbsInputChannel> id { get; set; }
        public TLChats Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            id = ObjectUtils.DeserializeVector<TLAbsInputChannel>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(id, bw);
        }

        public override void deserializeResponse(BinaryReader br)
        {
            Response = (TLChats) ObjectUtils.DeserializeObject(br);
        }
    }
}