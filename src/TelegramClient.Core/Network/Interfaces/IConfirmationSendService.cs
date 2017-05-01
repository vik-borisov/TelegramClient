namespace TelegramClient.Core.Network.Interfaces
{
    internal interface IConfirmationSendService
    {
        void StartSendingConfirmation();

        void AddForSend(ulong messageId);
    }
}