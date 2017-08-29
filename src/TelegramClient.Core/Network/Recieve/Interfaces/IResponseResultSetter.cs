namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System;

    using OpenTl.Schema;

    internal interface IResponseResultSetter
    {
        void ReturnResult(long requestId, IObject obj);

        void ReturnException(long requestId, Exception exception);
    }
}