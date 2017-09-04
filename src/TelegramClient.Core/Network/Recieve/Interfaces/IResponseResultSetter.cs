namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System;

    using OpenTl.Schema;

    internal interface IResponseResultSetter
    {
        void ReturnResult(long requestId, object obj);

        void ReturnException(long requestId, Exception exception);
    }
}