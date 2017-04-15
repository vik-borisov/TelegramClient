using System;
using System.Threading.Tasks;
using TelegramClient.Entities.TL.Contacts;

namespace TelegramClient.Core
{
    public static class ContactExtentions
    {
        public static async Task<TlContacts> GetContactsAsync(this ITelegramClient client)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var req = new TlRequestGetContacts { Hash = "" };

            return await client.SendRequestAsync<TlContacts>(req);
        }

    }
}