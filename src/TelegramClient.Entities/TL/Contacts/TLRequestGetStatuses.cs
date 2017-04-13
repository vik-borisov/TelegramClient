using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-995929106)]
    public class TlRequestGetStatuses : TlMethod
    {
        public override int Constructor => -995929106;

        public TlVector<TlContactStatus> Response { get; set; }


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
            Response = ObjectUtils.DeserializeVector<TlContactStatus>(br);
        }
    }
}