using System.IO;

namespace TelegramClient.Entities.TL
{
    [TlObject(-1704251862)]
    public class TlConfig : TlObject
    {
        public override int Constructor => -1704251862;

        public int Flags { get; set; }
        public int Date { get; set; }
        public int Expires { get; set; }
        public bool TestMode { get; set; }
        public int ThisDc { get; set; }
        public TlVector<TlDcOption> DcOptions { get; set; }
        public int ChatSizeMax { get; set; }
        public int MegagroupSizeMax { get; set; }
        public int ForwardedCountMax { get; set; }
        public int OnlineUpdatePeriodMs { get; set; }
        public int OfflineBlurTimeoutMs { get; set; }
        public int OfflineIdleTimeoutMs { get; set; }
        public int OnlineCloudTimeoutMs { get; set; }
        public int NotifyCloudDelayMs { get; set; }
        public int NotifyDefaultDelayMs { get; set; }
        public int ChatBigSize { get; set; }
        public int PushChatPeriodMs { get; set; }
        public int PushChatLimit { get; set; }
        public int SavedGifsLimit { get; set; }
        public int EditTimeLimit { get; set; }
        public int RatingEDecay { get; set; }
        public int StickersRecentLimit { get; set; }
        public int? TmpSessions { get; set; }
        public TlVector<TlDisabledFeature> DisabledFeatures { get; set; }


        public void ComputeFlags()
        {
            Flags = 0;
            Flags = TmpSessions != null ? Flags | 1 : Flags & ~1;
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Date = br.ReadInt32();
            Expires = br.ReadInt32();
            TestMode = BoolUtil.Deserialize(br);
            ThisDc = br.ReadInt32();
            DcOptions = ObjectUtils.DeserializeVector<TlDcOption>(br);
            ChatSizeMax = br.ReadInt32();
            MegagroupSizeMax = br.ReadInt32();
            ForwardedCountMax = br.ReadInt32();
            OnlineUpdatePeriodMs = br.ReadInt32();
            OfflineBlurTimeoutMs = br.ReadInt32();
            OfflineIdleTimeoutMs = br.ReadInt32();
            OnlineCloudTimeoutMs = br.ReadInt32();
            NotifyCloudDelayMs = br.ReadInt32();
            NotifyDefaultDelayMs = br.ReadInt32();
            ChatBigSize = br.ReadInt32();
            PushChatPeriodMs = br.ReadInt32();
            PushChatLimit = br.ReadInt32();
            SavedGifsLimit = br.ReadInt32();
            EditTimeLimit = br.ReadInt32();
            RatingEDecay = br.ReadInt32();
            StickersRecentLimit = br.ReadInt32();
            if ((Flags & 1) != 0)
                TmpSessions = br.ReadInt32();
            else
                TmpSessions = null;

            DisabledFeatures = ObjectUtils.DeserializeVector<TlDisabledFeature>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Date);
            bw.Write(Expires);
            BoolUtil.Serialize(TestMode, bw);
            bw.Write(ThisDc);
            ObjectUtils.SerializeObject(DcOptions, bw);
            bw.Write(ChatSizeMax);
            bw.Write(MegagroupSizeMax);
            bw.Write(ForwardedCountMax);
            bw.Write(OnlineUpdatePeriodMs);
            bw.Write(OfflineBlurTimeoutMs);
            bw.Write(OfflineIdleTimeoutMs);
            bw.Write(OnlineCloudTimeoutMs);
            bw.Write(NotifyCloudDelayMs);
            bw.Write(NotifyDefaultDelayMs);
            bw.Write(ChatBigSize);
            bw.Write(PushChatPeriodMs);
            bw.Write(PushChatLimit);
            bw.Write(SavedGifsLimit);
            bw.Write(EditTimeLimit);
            bw.Write(RatingEDecay);
            bw.Write(StickersRecentLimit);
            if ((Flags & 1) != 0)
                bw.Write(TmpSessions.Value);
            ObjectUtils.SerializeObject(DisabledFeatures, bw);
        }
    }
}