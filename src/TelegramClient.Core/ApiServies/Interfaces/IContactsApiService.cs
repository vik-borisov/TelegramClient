namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading.Tasks;

    using OpenTl.Schema.Contacts;

    public interface IContactsApiService
    {
        Task<IContacts> GetContactsAsync();

        Task<IFound> SearchUserAsync(string q, int limit = 10);
    }
}