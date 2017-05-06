namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System;

    internal interface IResponseResultSetter
    {
        void ReturnResult(ulong requestId, byte[] reader);

        void ReturnException(ulong requestId, Exception exception);
    }
}