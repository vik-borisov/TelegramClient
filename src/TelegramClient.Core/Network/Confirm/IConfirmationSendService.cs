namespace TelegramClient.Core.Network.Confirm
{
    internal interface IConfirmationSendService
    {
        void AddForSend(long messageId);
    }
}