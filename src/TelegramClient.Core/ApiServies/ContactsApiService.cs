namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Threading.Tasks;

    using OpenTl.Schema.Contacts;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IContactsApiService))]
    internal class ContactsApiService : IContactsApiService
    {
        public IAuthApiService AuthApiService { get; set; }

        public ISenderService SenderService { get; set; }

        public async Task<IContacts> GetContactsAsync()
        {
            EnsureUserAuthorized();

            var req = new RequestGetContacts { Hash = 0 };

            return await SenderService.SendRequestAsync(req).ConfigureAwait(false);
        }

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found; By default the limit is 10.
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <returns></returns>
        public Task<IFound> SearchUserAsync(string q)
        {
            return SearchUserAsync(q, 10);
        }

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        public async Task<IFound> SearchUserAsync(string q, int limit)
        {
            EnsureUserAuthorized();

            var r = new RequestSearch
                    {
                        Q = q,
                        Limit = limit
                    };

            return await SenderService.SendRequestAsync(r).ConfigureAwait(false);
        }

        private void EnsureUserAuthorized()
        {
            if (!AuthApiService.IsUserAuthorized())
            {
                throw new InvalidOperationException("Authorize user first!");
            }
        }

    }
}