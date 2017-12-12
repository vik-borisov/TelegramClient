namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using System;

    internal interface IRecievingService : IDisposable
    {
        void StartReceiving();
    }
}