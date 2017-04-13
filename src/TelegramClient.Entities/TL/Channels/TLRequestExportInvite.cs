using System.IO;

namespace TelegramClient.Entities.TL.Channels
{
    [TlObject(-950663035)]
    public class TlRequestExportInvite : TlMethod
    {
        public override int Constructor => -950663035;

        public TlAbsInputChannel Channel { get; set; }
        public TlAbsExportedChatInvite Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TlAbsInputChannel) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlAbsExportedChatInvite) ObjectUtils.DeserializeObject(br);
        }
    }
}