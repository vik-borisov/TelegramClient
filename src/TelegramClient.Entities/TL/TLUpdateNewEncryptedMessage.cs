using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(314359194)]
    public class TlUpdateNewEncryptedMessage : TlAbsUpdate
    {
        public override int Constructor => 314359194;

        public TlAbsEncryptedMessage Message { get; set; }
        public int Qts { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Message = (TlAbsEncryptedMessage) ObjectUtils.DeserializeObject(br);
            Qts = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Message, bw);
            bw.Write(Qts);
        }
    }
}