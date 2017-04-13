using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1990644519)]
    public class TlMessageEntityTextUrl : TlAbsMessageEntity
    {
        public override int Constructor => 1990644519;

        public int Offset { get; set; }
        public int Length { get; set; }
        public string Url { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Offset = br.ReadInt32();
            Length = br.ReadInt32();
            Url = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Offset);
            bw.Write(Length);
            StringUtil.Serialize(Url, bw);
        }
    }
}