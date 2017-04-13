using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(94983360)]
    public class TlRequestReceivedMessages : TlMethod
    {
        public override int Constructor => 94983360;

        public int MaxId { get; set; }
        public TlVector<TlReceivedNotifyMessage> Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            MaxId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(MaxId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = ObjectUtils.DeserializeVector<TlReceivedNotifyMessage>(br);
        }
    }
}