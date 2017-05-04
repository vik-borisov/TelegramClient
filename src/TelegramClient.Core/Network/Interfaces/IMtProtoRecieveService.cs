namespace TelegramClient.Core.Network.Interfaces
{
    internal interface IMtProtoRecieveService
    {
        void StartReceiving();

        void StopRecieving();
    }
}