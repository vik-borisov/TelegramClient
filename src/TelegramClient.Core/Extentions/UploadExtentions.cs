using System.Threading.Tasks;
using TelegramClient.Core.Network;
using TelegramClient.Entities.TL;
using TelegramClient.Entities.TL.Auth;
using TelegramClient.Entities.TL.Upload;
using TlAuthorization = TelegramClient.Entities.TL.TlAuthorization;

namespace TelegramClient.Core
{
    using BarsGroup.CodeGuard;
    using BarsGroup.CodeGuard.Validators;

    public static class UploadExtentions
    {
        public static async Task<TlFile> GetFile(this ITelegramClient client, TlAbsInputFileLocation location, int filePartSize, int offset = 0)
        {
            try
            {
                return await client.SendRequestAsync<TlFile>(new TlRequestGetFile
                {
                    Location = location,
                    Limit = filePartSize,
                    Offset = offset
                });
            }
            catch (FileMigrationException ex)
            {
                var exportedAuth =
                    await client.SendRequestAsync<TlExportedAuthorization>(new TlRequestExportAuthorization {DcId = ex.Dc});

                var clientSettings = client.GetSettings();
                Guard.That(clientSettings).IsNotNull();
                Guard.That(clientSettings.Session).IsNotNull();

                var authKey = clientSettings.Session.AuthKey;
                var timeOffset = clientSettings.Session.TimeOffset;
                var serverAddress = clientSettings.Session.ServerAddress;
                var serverPort = clientSettings.Session.Port;

                await client.ReconnectToDcAsync(ex.Dc);
                await client.SendRequestAsync<TlAuthorization>(new TlRequestImportAuthorization
                {
                    Bytes = exportedAuth.Bytes,
                    Id = exportedAuth.Id
                });
               var result = await client.GetFile(location, filePartSize, offset);

                clientSettings.Session.AuthKey = authKey;
                clientSettings.Session.TimeOffset = timeOffset;
                clientSettings.Session.ServerAddress = serverAddress;
                clientSettings.Session.Port = serverPort;
                await client.ConnectAsync();

                return result;
            }

        }

    }
}