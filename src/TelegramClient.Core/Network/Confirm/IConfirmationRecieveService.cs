namespace TelegramClient.Core.Network.Confirm
{
    using System;
    using System.Threading.Tasks;

    internal interface IConfirmationRecieveService
    {
        Task WaitForConfirm(long messageId);

        void ConfirmRequest(long requestId);

        void RequestWithException(long requestId, Exception exception);
    }
}