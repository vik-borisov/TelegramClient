namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using OpenTl.Schema;

    public interface IConnectApiService: IDisposable
    {
        Task ConnectAsync();

        Task LogOut();

        Task<IPong> PingAsync();
        
        Task ReAuthenticateAsync();

        Task ReconnectToDcAsync(int dcId);
    }
}