using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(916930423)]
    public class TlRequestReadMessageContents : TlMethod
    {
        public override int Constructor => 916930423;

        public TlVector<int> Id { get; set; }
        public TlAffectedMessages Response { get; set; }


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
            Response = (TlAffectedMessages) ObjectUtils.DeserializeObject(br);
        }
    }
}