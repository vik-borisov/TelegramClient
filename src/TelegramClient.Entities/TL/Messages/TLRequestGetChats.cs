using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(1013621127)]
    public class TlRequestGetChats : TlMethod
    {
        public override int Constructor => 1013621127;

        public TlVector<int> Id { get; set; }
        public TlChats Response { get; set; }


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
            Response = (TlChats) ObjectUtils.DeserializeObject(br);
        }
    }
}