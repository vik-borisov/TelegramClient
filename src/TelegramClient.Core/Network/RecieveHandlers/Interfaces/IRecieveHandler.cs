namespace TelegramClient.Core.Network.RecieveHandlers.Interfaces
{
    using System.IO;

    public interface IRecieveHandler
    {
        int[] HandleCodes { get; }

        byte[] HandleResponce(int code, BinaryReader reader);
    }
}