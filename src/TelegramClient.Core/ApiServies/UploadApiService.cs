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
        private readonly int DownloadPhotoPartSize = 64 * 1024; // 64kb for photo

        private readonly int DownloadDocumentPartSize = 128 * 1024; // 128kb for document

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
                           });
            }
            catch (FileMigrationException ex)
            {
                var exportedAuth = (TExportedAuthorization)await SenderService.SendRequestAsync(
                                                               new RequestExportAuthorization
                                                               {
                                                                   DcId = ex.Dc
                                                               });

                var authKey = ClientSettings.Session.AuthKey;
                var timeOffset = ClientSettings.Session.TimeOffset;
                var serverAddress = ClientSettings.Session.ServerAddress;
                var serverPort = ClientSettings.Session.Port;

                await ConnectApiService.ReconnectToDcAsync(ex.Dc);
                await SenderService.SendRequestAsync(
                    new RequestImportAuthorization
                    {
                        Bytes = exportedAuth.Bytes,
                        Id = exportedAuth.Id
                    });
                var result = await GetFile(location, offset);

                ClientSettings.Session.AuthKey = authKey;
                ClientSettings.Session.TimeOffset = timeOffset;
                ClientSettings.Session.ServerAddress = serverAddress;
                ClientSettings.Session.Port = serverPort;
                await ConnectApiService.ConnectAsync();

                return result;
            }
        }

        public async Task<IInputFile> UploadFile(string name, StreamReader reader)
        {
            const long TenMb = 10 * 1024 * 1024;
            var isBigFileUpload = reader.BaseStream.Length >= TenMb;

            var file = GetFile(reader);
            var fileParts = GetFileParts(file);

            var partNumber = 0;
            var partsCount = fileParts.Count;
            var fileId = BitConverter.ToInt64(TlHelpers.GenerateRandomBytes(8), 0);
            while (fileParts.Count != 0)
            {
                var part = fileParts.Dequeue();

                if (isBigFileUpload)
                    await SenderService.SendRequestAsync(
                        new RequestSaveBigFilePart
                        {
                            FileId = fileId,
                            FilePart = partNumber,
                            Bytes = part,
                            FileTotalParts = partsCount
                        });
                else
                    await SenderService.SendRequestAsync(
                        new RequestSaveFilePart
                        {
                            FileId = fileId,
                            FilePart = partNumber,
                            Bytes = part
                        });
                partNumber++;
            }

            if (isBigFileUpload)
                return new TInputFileBig
                       {
                           Id = fileId,
                           Name = name,
                           Parts = partsCount
                       };
            return new TInputFile
                   {
                       Id = fileId,
                       Name = name,
                       Parts = partsCount,
                       Md5Checksum = GetFileHash(file)
                   };
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

        private static byte[] GetFile(StreamReader reader)
        {
            var file = new byte[reader.BaseStream.Length];

            using (reader)
            {
                reader.BaseStream.Read(file, 0, (int)reader.BaseStream.Length);
            }

            return file;
        }

        private static Queue<byte[]> GetFileParts(byte[] file)
        {
            var fileParts = new Queue<byte[]>();

            const int maxFilePart = 512 * 1024;

            using (var stream = new MemoryStream(file))
            {
                while (stream.Position != stream.Length)
                    if (stream.Length - stream.Position > maxFilePart)
                    {
                        var temp = new byte[maxFilePart];
                        stream.Read(temp, 0, maxFilePart);
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