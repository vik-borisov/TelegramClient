namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System;

    internal interface IResponseResultSetter
    {
        void ReturnResult(ulong requestId, byte[] bytes);

        void ReturnException(ulong requestId, Exception exception);
    }
}