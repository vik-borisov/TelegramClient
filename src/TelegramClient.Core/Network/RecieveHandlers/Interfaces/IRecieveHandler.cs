namespace TelegramClient.Core.Network.RecieveHandlers.Interfaces
{
    using System;

    using OpenTl.Schema;

    public interface IRecieveHandler
    {
        Type[] HandleCodes { get; }

        void HandleResponce(IObject obj);
    }
}