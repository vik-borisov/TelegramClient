using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(1769565673)]
    public class TlRequestInitConnection : TlMethod
    {
        public override int Constructor => 1769565673;

        public int ApiId { get; set; }
        public string DeviceModel { get; set; }
        public string SystemVersion { get; set; }
        public string AppVersion { get; set; }
        public string LangCode { get; set; }
        public TlObject Query { get; set; }
        public TlObject Response { get; set; }


        public void ComputeFlags()
        {
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ApiId = br.ReadInt32();
            DeviceModel = StringUtil.Deserialize(br);
            SystemVersion = StringUtil.Deserialize(br);
            AppVersion = StringUtil.Deserialize(br);
            LangCode = StringUtil.Deserialize(br);
            Query = (TlObject) ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ApiId);
            StringUtil.Serialize(DeviceModel, bw);
            StringUtil.Serialize(SystemVersion, bw);
            StringUtil.Serialize(AppVersion, bw);
            StringUtil.Serialize(LangCode, bw);
            ObjectUtils.SerializeObject(Query, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlObject) ObjectUtils.DeserializeObject(br);
        }
    }
}