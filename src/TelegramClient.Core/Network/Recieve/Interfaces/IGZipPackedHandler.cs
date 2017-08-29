namespace TelegramClient.Core.Network.Recieve.Interfaces
{
    using OpenTl.Schema;

    internal interface IGZipPackedHandler 
    {
        IObject HandleGZipPacked(TgZipPacked obj);
    }
}