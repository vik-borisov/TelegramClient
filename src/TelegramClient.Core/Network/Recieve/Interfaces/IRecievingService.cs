namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    internal interface IRecievingService
    {
        void StartReceiving();

        void StopRecieving();
    }
}