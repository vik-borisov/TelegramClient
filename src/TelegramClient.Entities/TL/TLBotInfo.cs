using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1729618630)]
    public class TlBotInfo : TlObject
    {
        public override int Constructor => -1729618630;

        public int UserId { get; set; }
        public string Description { get; set; }
        public TlVector<TlBotCommand> Commands { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt32();
            Description = StringUtil.Deserialize(br);
            Commands = ObjectUtils.DeserializeVector<TlBotCommand>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            StringUtil.Serialize(Description, bw);
            ObjectUtils.SerializeObject(Commands, bw);
        }
    }
}