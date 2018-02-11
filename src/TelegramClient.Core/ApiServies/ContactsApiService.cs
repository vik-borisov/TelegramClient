﻿namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using OpenTl.Schema.Contacts;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;

    [SingleInstance(typeof(IContactsApiService))]
    internal class ContactsApiService : IContactsApiService
    {
        public IAuthApiService AuthApiService { get; set; }

        public ISenderService SenderService { get; set; }

        public async Task<IContacts> GetContactsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureUserAuthorized();

            var req = new RequestGetContacts { Hash = 0 };

            return await SenderService.SendRequestAsync(req, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;</summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<IFound> SearchUserAsync(string q, int limit = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            EnsureUserAuthorized();

            var r = new RequestSearch
                    {
                        Q = q,
                        Limit = limit
                    };

            return await SenderService.SendRequestAsync(r, cancellationToken).ConfigureAwait(false);
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