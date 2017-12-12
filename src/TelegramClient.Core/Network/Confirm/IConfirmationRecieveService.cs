namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Threading.Tasks;

    internal interface IConfirmationRecieveService
    {
        void ConfirmRequest(long requestId);

        void RequestWithException(long requestId, Exception exception);

        Task WaitForConfirm(long messageId);
    }
}