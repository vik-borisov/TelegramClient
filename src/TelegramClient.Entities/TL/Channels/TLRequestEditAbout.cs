using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(333610782)]
    public class TlRequestEditAbout : TlMethod
    {
        public override int Constructor => 333610782;

        public TlAbsInputChannel Channel { get; set; }
        public string About { get; set; }
        public bool Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
            About = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            StringUtil.Serialize(About, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}