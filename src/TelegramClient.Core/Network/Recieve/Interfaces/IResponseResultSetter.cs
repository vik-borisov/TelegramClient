namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System;

    internal interface IResponseResultSetter
    {
        void ReturnException(long requestId, Exception exception);
        
        void ReturnException(Exception exception);

        void ReturnResult(long requestId, object obj);
    }
}