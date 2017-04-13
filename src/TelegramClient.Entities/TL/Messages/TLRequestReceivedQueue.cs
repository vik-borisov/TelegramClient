using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1436924774)]
    public class TlRequestReceivedQueue : TlMethod
    {
        public override int Constructor => 1436924774;

        public int MaxQts { get; set; }
        public TlVector<long> Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            MaxQts = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(MaxQts);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = ObjectUtils.DeserializeVector<long>(br);
        }
    }
}