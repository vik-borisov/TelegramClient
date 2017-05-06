namespace TelegramClient.Core.Network.Confirm
{
    internal interface IConfirmationSendService
    {
        void StartSendingConfirmation();

        void AddForSend(ulong messageId);
    }
}