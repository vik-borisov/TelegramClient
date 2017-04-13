using System.IO;

namespace TelegramClient.Entities.TL.Storage
{
    [TlObject(8322574)]
    public class TlFileJpeg : TlAbsFileType
    {
        public override int Constructor => 8322574;


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