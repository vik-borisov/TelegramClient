using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1109588596)]
    public class TlRequestGetMessages : TlMethod
    {
        public override int Constructor => 1109588596;

        public TlVector<int> Id { get; set; }
        public TlAbsMessages Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}