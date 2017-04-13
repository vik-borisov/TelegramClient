using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(164303470)]
    public class TlRequestCreateChat : TlMethod
    {
        public override int Constructor => 164303470;

        public TlVector<TlAbsInputUser> Users { get; set; }
        public string Title { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Users = ObjectUtils.DeserializeVector<TlAbsInputUser>(br);
            Title = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Users, bw);
            StringUtil.Serialize(Title, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}