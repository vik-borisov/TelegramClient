using System.IO;

namespace TelegramClient.Entities.TL.Auth
{
    [TlObject(-855308010)]
    public class TlAuthorization : TlObject
    {
        public override int Constructor => -855308010;

        public int Flags { get; set; }
        public int? TmpSessions { get; set; }
        public TlAbsUser User { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = TmpSessions != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            if ((Flags & 1) != 0)
                TmpSessions = br.ReadInt32();
            else
                TmpSessions = null;

            User = (TlAbsUser) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            if ((Flags & 1) != 0)
                bw.Write(TmpSessions.Value);
            ObjectUtils.SerializeObject(User, bw);
        }
    }
}