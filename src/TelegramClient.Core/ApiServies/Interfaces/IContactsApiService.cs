namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading.Tasks;

    using OpenTl.Schema.Contacts;

    public interface IContactsApiService
    {
        Task<IContacts> GetContactsAsync();

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        Task<IFound> SearchUserAsync(string q, int limit);

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found; By default the limit is 10.
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <returns></returns>
        Task<IFound> SearchUserAsync(string q);
    }
}