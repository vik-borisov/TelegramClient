using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(-1987579119)]
    public class TlAppUpdate : TlAbsAppUpdate
    {
        public override int Constructor => -1987579119;

        public int Id { get; set; }
        public bool Critical { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            Critical = BoolUtil.Deserialize(br);
            Url = StringUtil.Deserialize(br);
            Text = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            BoolUtil.Serialize(Critical, bw);
            StringUtil.Serialize(Url, bw);
            StringUtil.Serialize(Text, bw);
        }
    }
}