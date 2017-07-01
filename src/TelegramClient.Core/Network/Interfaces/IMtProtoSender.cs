namespace TelegramClient.Core.Network.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using TelegramClient.Entities;

    internal interface IMtProtoSender
    {
        Task<Tuple<Task, ulong>> Send(TlMethod request);
    }
}