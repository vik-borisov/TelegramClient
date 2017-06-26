namespace TelegramClient.Core.Network.Confirm
{
    internal interface IConfirmationSendService
    {
        void StartSendingConfirmation();

        void AddForSend(long messageId);
    }
}