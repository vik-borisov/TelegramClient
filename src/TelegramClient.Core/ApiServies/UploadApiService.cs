namespace TelegramClient.Core.ApiServies
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Auth;
    using OpenTl.Schema.Upload;

    using TelegramClient.Core.ApiServies.Interfaces;
    using TelegramClient.Core.IoC;
    using TelegramClient.Core.Network.Exceptions;
    using TelegramClient.Core.Settings;
    using TelegramClient.Core.Utils;

    [SingleInstance(typeof(IUploadApiService))]
    internal class UploadApiService : IUploadApiService
    {
        private readonly int DownloadDocumentPartSize = 128 * 1024; // 128kb for document

        private readonly int DownloadPhotoPartSize = 64 * 1024; // 64kb for photo

        public ISenderService SenderService { get; set; }

        public IConnectApiService ConnectApiService { get; set; }

        public IClientSettings ClientSettings { get; set; }

        public async Task<IFile> GetFile(IInputFileLocation location, int offset = 0)
        {
            int filePartSize;
            if (location is TInputDocumentFileLocation)
            {
                filePartSize = DownloadDocumentPartSize;
            }
            else
            {
                filePartSize = DownloadPhotoPartSize;
            }

            try
            {
                return await SenderService.SendRequestAsync(
                           new RequestGetFile
                           {
                               Location = location,
                               Limit = filePartSize,
                               Offset = offset
                           }).ConfigureAwait(false);
            }
            catch (FileMigrationException ex)
            {
                var exportedAuth = (TExportedAuthorization)await SenderService.SendRequestAsync(
                                                               new RequestExportAuthorization
                                                               {
                                                                   DcId = ex.Dc
                                                               }).ConfigureAwait(false);

                var authKey = ClientSettings.Session.AuthKey;
                var timeOffset = ClientSettings.Session.TimeOffset;
                var serverAddress = ClientSettings.Session.ServerAddress;
                var serverPort = ClientSettings.Session.Port;

                await ConnectApiService.ReconnectToDcAsync(ex.Dc).ConfigureAwait(false);
                await SenderService.SendRequestAsync(
                    new RequestImportAuthorization
                    {
                        Bytes = exportedAuth.Bytes,
                        Id = exportedAuth.Id
                    }).ConfigureAwait(false);
                var result = await GetFile(location, offset).ConfigureAwait(false);

                ClientSettings.Session.AuthKey = authKey;
                ClientSettings.Session.TimeOffset = timeOffset;
                ClientSettings.Session.ServerAddress = serverAddress;
                ClientSettings.Session.Port = serverPort;
                await ConnectApiService.ConnectAsync().ConfigureAwait(false);

                return result;
            }
        }

        public async Task<IInputFile> UploadFile(string name, StreamReader reader)
        {
            const long TenMb = 10 * 1024 * 1024;
            var isBigFileUpload = reader.BaseStream.Length >= TenMb;

            var file = await GetFile(reader);
            var fileParts = GetFileParts(file);

            var partNumber = 0;
            var partsCount = fileParts.Count;
            var fileId = BitConverter.ToInt64(TlHelpers.GenerateRandomBytes(8), 0);
            while (fileParts.Count != 0)
            {
                var part = fileParts.Dequeue();

                if (isBigFileUpload)
                {
                    await SenderService.SendRequestAsync(
                        new RequestSaveBigFilePart
                        {
                            FileId = fileId,
                            FilePart = partNumber,
                            Bytes = part,
                            FileTotalParts = partsCount
                        }).ConfigureAwait(false);
                }
                else
                {
                    await SenderService.SendRequestAsync(
                        new RequestSaveFilePart
                        {
                            FileId = fileId,
                            FilePart = partNumber,
                            Bytes = part
                        }).ConfigureAwait(false);
                }
                partNumber++;
            }

            if (isBigFileUpload)
            {
                return new TInputFileBig
                       {
                           Id = fileId,
                           Name = name,
                           Parts = partsCount
                       };
            }
            return new TInputFile
                   {
                       Id = fileId,
                       Name = name,
                       Parts = partsCount,
                       Md5Checksum = GetFileHash(file)
                   };
        }

        private static async Task<byte[]> GetFile(StreamReader reader)
        {
            var file = new byte[reader.BaseStream.Length];

            using (reader)
            {
                await reader.BaseStream.ReadAsync(file, 0, (int)reader.BaseStream.Length).ConfigureAwait(false);
            }

            return file;
        }

        private static string GetFileHash(byte[] data)
        {
            string md5Checksum;
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                var hashResult = new StringBuilder(hash.Length * 2);

                foreach (var t in hash)
                {
                    hashResult.Append(t.ToString("x2"));
                }

                md5Checksum = hashResult.ToString();
            }

            return md5Checksum;
        }

        private static Queue<byte[]> GetFileParts(byte[] file)
        {
            var fileParts = new Queue<byte[]>();

            const int MaxFilePart = 512 * 1024;

            using (var stream = new MemoryStream(file))
            {
                while (stream.Position != stream.Length)
                    if (stream.Length - stream.Position > MaxFilePart)
                    {
                        var temp = new byte[MaxFilePart];
                        stream.Read(temp, 0, MaxFilePart);
                        fileParts.Enqueue(temp);
                    }
                    else
                    {
                        var length = stream.Length - stream.Position;
                        var temp = new byte[length];
                        stream.Read(temp, 0, (int)length);
                        fileParts.Enqueue(temp);
                    }
            }

            return fileParts;
        }
    }
}