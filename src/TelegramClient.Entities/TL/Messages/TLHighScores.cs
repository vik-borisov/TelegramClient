using System.IO;

namespace TelegramClient.Entities.TL.Messages
{
    [TlObject(-1707344487)]
    public class TlHighScores : TlObject
    {
        public override int Constructor => -1707344487;

        public TlVector<TlHighScore> Scores { get; set; }
        public TlVector<TlAbsUser> Users { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Scores = ObjectUtils.DeserializeVector<TlHighScore>(br);
            Users = ObjectUtils.DeserializeVector<TlAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Scores, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}