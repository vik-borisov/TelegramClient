using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1503425638)]
    public class TlMessageActionChatCreate : TlAbsMessageAction
    {
        public override int Constructor => -1503425638;

        public string Title { get; set; }
        public TlVector<int> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Title = StringUtil.Deserialize(br);
            Users = ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Title, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}