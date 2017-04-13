using System.IO;

namespace TelegramClient.Entities.TL.Contacts
{
    [TlObject(301470424)]
    public class TlRequestSearch : TlMethod
    {
        public override int Constructor => 301470424;

        public string Q { get; set; }
        public int Limit { get; set; }
        public TlFound Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Q = StringUtil.Deserialize(br);
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Q, bw);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlFound) ObjectUtils.DeserializeObject(br);
        }
    }
}