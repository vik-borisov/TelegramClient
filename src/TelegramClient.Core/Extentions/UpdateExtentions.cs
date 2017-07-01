namespace TelegramClient.Core
{
    using System.Threading.Tasks;

    using OpenTl.Schema.Updates;

    public static class UpdateExtentions
    {
        public static async Task<IState> GetCurrentState(this ITelegramClient telegramClient)
        {
            return await telegramClient.SendRequestAsync(new RequestGetState());
        }

        public static async Task<IDifference> GetUpdates(this ITelegramClient telegramClient, IState currentState)
        {
            var getDiffRequest = new RequestGetDifference
                                 {
                                     Pts = currentState.Pts,
                                     Qts = currentState.Qts,
                                     Date = currentState.Date
                                 };

            return await telegramClient.SendRequestAsync(getDiffRequest);
        }
    }
}