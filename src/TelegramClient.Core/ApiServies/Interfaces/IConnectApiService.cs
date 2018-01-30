namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading.Tasks;

    public interface IConnectApiService
    {
        Task ConnectAsync();

        Task ReAuthenticateAsync();

        Task ReconnectToDcAsync(int dcId);

        Task LogOut();
    }
}