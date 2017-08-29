namespace TelegramClient.Core.Network.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using OpenTl.Schema;

    internal interface IMtProtoSender
    {
        Task<Tuple<Task, long>> Send(IObject obj);
    }
}