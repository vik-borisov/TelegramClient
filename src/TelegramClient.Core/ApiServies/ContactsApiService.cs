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

            var req = new RequestGetContacts { Hash = "" };

            return await SenderService.SendRequestAsync(req);
        }

        /// <summary>
        ///     Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        public async Task<IFound> SearchUserAsync(string q, int limit = 10)
        {
            EnsureUserAuthorized();

            var r = new RequestSearch
                    {
                        Q = q,
                        Limit = limit
                    };

            return await SenderService.SendRequestAsync(r);
        }

        private void EnsureUserAuthorized()
        {
            if (!AuthApiService.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");
        }

    }
}