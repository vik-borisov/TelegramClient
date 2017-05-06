namespace TelegramClient.Core.Network.RecieveHandlers.Interfaces
{
    using System.Collections.Generic;
    using System.IO;

    public interface IRecieveHandler
    {
        uint ResponceCode { get; }

        IEnumerable<byte[]> HandleResponce(BinaryReader reader);
    }
}