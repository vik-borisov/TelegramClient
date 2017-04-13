using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1697798976)]
    public class TlPeerNotifySettings : TlAbsPeerNotifySettings
    {
        public override int Constructor => -1697798976;

        public int Flags { get; set; }
        public bool ShowPreviews { get; set; }
        public bool Silent { get; set; }
        public int MuteUntil { get; set; }
        public string Sound { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = ShowPreviews ? Flags | 1 : Flags & ~1;
            Flags = Silent ? Flags | 2 : Flags & ~2;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            ShowPreviews = (Flags & 1) != 0;
            Silent = (Flags & 2) != 0;
            MuteUntil = br.ReadInt32();
            Sound = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);


            bw.Write(MuteUntil);
            StringUtil.Serialize(Sound, bw);
        }
    }
}