using System;
using System.Threading.Tasks;

namespace TelegramClient.Core
{
    using OpenTl.Schema.Contacts;

    public static class ContactExtentions
    {
        public static async Task<IContacts> GetContactsAsync(this ITelegramClient client)
        {
            if (!client.IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            var req = new RequestGetContacts { Hash = "" };

            return await client.SendRequestAsync(req);
        }

    }
}