using System.IO;
using TelegramClient.Entities.TL.Auth;

namespace TelegramClient.Entities.TL.Account
{
    [TlObject(149257707)]
    public class TlRequestSendChangePhoneCode : TlMethod
    {
        public override int Constructor => 149257707;

        public int Flags { get; set; }
        public bool AllowFlashcall { get; set; }
        public string PhoneNumber { get; set; }
        public bool? CurrentNumber { get; set; }
        public TlSentCode Response { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = AllowFlashcall ? Flags | 1 : Flags & ~1;
            Flags = CurrentNumber != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            AllowFlashcall = (Flags & 1) != 0;
            PhoneNumber = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                CurrentNumber = BoolUtil.Deserialize(br);
            else
                CurrentNumber = null;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);

            StringUtil.Serialize(PhoneNumber, bw);
            if ((Flags & 1) != 0)
                BoolUtil.Serialize(CurrentNumber.Value, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TlSentCode) ObjectUtils.DeserializeObject(br);
        }
    }
}