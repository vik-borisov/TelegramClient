namespace TelegramClient.Core.Network.Confirm
{
    using System;

    internal interface IConfirmationSendService : IDisposable
    {
        void AddForSend(long messageId);
    }
}