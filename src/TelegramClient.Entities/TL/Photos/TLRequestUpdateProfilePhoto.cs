using System.IO;

namespace TelegramClient.Entities.TL.Photos
{
    [TlObject(-256159406)]
    public class TlRequestUpdateProfilePhoto : TlMethod
    {
        public override int Constructor => -256159406;

        public TlAbsInputPhoto Id { get; set; }
        public TlAbsUserProfilePhoto Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TlAbsInputPhoto) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsUserProfilePhoto) ObjectUtils.DeserializeObject(br);
        }
    }
}