﻿namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.IO;
    using System.Threading.Tasks;

    using OpenTl.Schema;
    using OpenTl.Schema.Upload;

    public interface IUploadApiService
    {
        Task<IFile> GetFile(IInputFileLocation location, int offset = 0);

        Task<IInputFile> UploadFile(string name, StreamReader reader);
    }
}