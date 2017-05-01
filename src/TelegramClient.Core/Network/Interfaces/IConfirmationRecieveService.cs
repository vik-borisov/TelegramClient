namespace TelegramClient.Core.Network.Interfaces
{
    using System;
    using System.Threading.Tasks;

    internal interface IConfirmationRecieveService
    {
        Task WaitForConfirm(ulong messageId);

        void ConfirmRequest(ulong requestId);

        void RequestWithException(ulong requestId, Exception exception);
    }
}