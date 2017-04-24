namespace TelegramClient.Core
{
    using System.Threading.Tasks;

    using TelegramClient.Entities.TL.Updates;

    public static class UpdateExtentions
    {
        public static async Task<TlState> GetCurrentState(this ITelegramClient telegramClient)
        {
            return await telegramClient.SendRequestAsync<TlState>(new TlRequestGetState());
        }

        public static async Task<TlAbsDifference> GetUpdates(this ITelegramClient telegramClient, TlState currentState)
        {
            var getDiffRequest = new TlRequestGetDifference
                                 {
                                     Pts = currentState.Pts,
                                     Qts = currentState.Qts,
                                     Date = currentState.Date
                                 };

            return await telegramClient.SendRequestAsync<TlAbsDifference>(getDiffRequest);
        }
    }
}