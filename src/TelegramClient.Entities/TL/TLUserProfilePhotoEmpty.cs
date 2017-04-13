using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1326562017)]
    public class TlUserProfilePhotoEmpty : TlAbsUserProfilePhoto
    {
        public override int Constructor => 1326562017;


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
        }
    }
}