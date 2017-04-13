using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-192332417)]
    public class TlRequestCreateChannel : TlMethod
    {
        public override int Constructor => -192332417;

        public int Flags { get; set; }
        public bool Broadcast { get; set; }
        public bool Megagroup { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public TlAbsUpdates Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = Broadcast ? Flags | 1 : Flags & ~1;
            Flags = Megagroup ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Broadcast = (Flags & 1) != 0;
            Megagroup = (Flags & 2) != 0;
            Title = StringUtil.Deserialize(br);
            About = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            StringUtil.Serialize(Title, bw);
            StringUtil.Serialize(About, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUpdates) ObjectUtils.DeserializeObject(br);
        }
    }
}