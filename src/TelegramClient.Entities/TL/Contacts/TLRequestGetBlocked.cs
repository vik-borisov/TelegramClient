using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(-176409329)]
    public class TlRequestGetBlocked : TlMethod
    {
        public override int Constructor => -176409329;

        public int Offset { get; set; }
        public int Limit { get; set; }
        public TlAbsBlocked Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Offset = br.ReadInt32();
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Offset);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsBlocked) ObjectUtils.DeserializeObject(br);
        }
    }
}