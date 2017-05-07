namespace TelegramClient.Core.Network.RecieveHandlers.Interfaces
{
    using System.IO;

    public interface IRecieveHandler
    {
        uint[] HandleCodes { get; }

        byte[] HandleResponce(uint code, BinaryReader reader);
    }
}