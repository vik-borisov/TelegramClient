using System.IO;

namespace TelegramClient.Entities.TL.Help
{
    [TlObject(415997816)]
    public class TlInviteText : TlObject
    {
        public override int Constructor => 415997816;

        public string Message { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Message = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Message, bw);
        }
    }
}