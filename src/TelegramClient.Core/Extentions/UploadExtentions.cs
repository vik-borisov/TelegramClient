using System.Threading.Tasks;
using TelegramClient.Core.Network;

namespace TelegramClient.Core
{
    using BarsGroup.CodeGuard;

    using OpenTl.Schema;
    using OpenTl.Schema.Auth;
    using OpenTl.Schema.Upload;

    using TelegramClient.Core.Network.Exceptions;

    public static class UploadExtentions
    {
        public static async Task<IFile> GetFile(this ITelegramClient client, IInputFileLocation location, int filePartSize, int offset = 0)
        {
            try
            {
                return await client.SendRequestAsync(new RequestGetFile
                {
                    Location = location,
                    Limit = filePartSize,
                    Offset = offset
                });
            }
            catch (FileMigrationException ex)
            {
                var exportedAuth = (TExportedAuthorization) await client.SendRequestAsync(new RequestExportAuthorization {DcId = ex.Dc});

                var clientSettings = client.GetSettings();
                Guard.That(clientSettings).IsNotNull();
                Guard.That(clientSettings.Session).IsNotNull();

                var authKey = clientSettings.Session.AuthKey;
                var timeOffset = clientSettings.Session.TimeOffset;
                var serverAddress = clientSettings.Session.ServerAddress;
                var serverPort = clientSettings.Session.Port;

                await client.ReconnectToDcAsync(ex.Dc);
                await client.SendRequestAsync(new RequestImportAuthorization
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