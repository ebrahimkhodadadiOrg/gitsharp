using Amazon.S3;
using BuildingBlocks.Exceptions;
using BuildingBlocks.FileUtility;
using Domain.Models.Share;
using FluentFTP;
using FluentStorage;
using FluentStorage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace FileManager
{
    public class FileProvider : IFileManager
    {
        IBlobStorage _storage;

        public FileProvider(IOptionsSnapshot<FileManagerSettings> options)
        {
            Initilize(options.Value.Providers);
        }
        private void Initilize(Providers options)
        {
            if (options is null)
                throw new Exception("config FileManagerSettings in appsettings");

            /// select file provider
            // AWS S3
            if (options?.AWS is not null)
            {
                var s3Config = new AmazonS3Config
                {
                    UseHttp = true,
                    ForcePathStyle = true,
                    ServiceURL = options.AWS.ServiceURL
                };
                _storage = StorageFactory.Blobs.AwsS3(
                   accessKeyId: options.AWS.AccessKeyId,
                   secretAccessKey: options.AWS.SecretAccessKey,
                   null,
                   bucketName: options.AWS.BucketName,
                   clientConfig: s3Config);
            }
            // SFTP & FTP
            else if (options?.FTP is not null)
            {
                var client = new AsyncFtpClient(options.FTP.Host,
                new NetworkCredential(options.FTP.UserName, options.FTP.Password),
                21);
                _storage = StorageFactory.Blobs.FtpFromFluentFtpClient(client);
            }
            // LocalDisk
            else if (!string.IsNullOrWhiteSpace(options?.LocalDisk))
                _storage = StorageFactory.Blobs.DirectoryFiles(options.LocalDisk);
            //InMemory
            else if (options?.InMemory ?? false)
                _storage = StorageFactory.Blobs.InMemory();
            else
                throw new Exception("config FileProvider in appsettings");
        }

        public async Task<IReadOnlyCollection<Blob>> GetAll(string fullPath, bool includeSub = true, string search = "", int? count = null,
            CancellationToken cancellationToken = default)
        {
            var options = new ListOptions() { FolderPath = fullPath };
            if (!string.IsNullOrWhiteSpace(search))
                options.FilePrefix = search;
            if (count.HasValue)
                options.MaxResults = count;

            options.Recurse = includeSub;

            return await _storage.ListAsync(options, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Blob>> GetAllFiles(string fullPath, bool includeSub = true, string search = "", int? count = null,
            CancellationToken cancellationToken = default)
        {
            var options = new ListOptions() { FolderPath = fullPath };
            if (!string.IsNullOrWhiteSpace(search))
                options.FilePrefix = search;

            options.Recurse = includeSub;

            return await _storage.ListFilesAsync(options, cancellationToken);
        }

        public async Task<string> CreateFolderAsync(string folderPath, CancellationToken cancellationToken = default)
        {
            await _storage.CreateFolderAsync(folderPath, cancellationToken: cancellationToken);

            return folderPath;
        }

        public async Task UploadImageAsync(byte[] data, string path, CancellationToken cancellationToken = default)
        {
            await _storage.WriteAsync(path, data, cancellationToken: cancellationToken);
        }

        public async Task<string> UploadAsync(IFormFile file, string path, CancellationToken cancellationToken = default)
        {
            path = $"{path}{file.FileName}";

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);
            await _storage.WriteAsync(path, memoryStream, cancellationToken: cancellationToken);

            return path;
        }

        public async Task<FileStreamResult> DownloadAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var fileStream = await _storage.OpenReadAsync(filePath, cancellationToken);

            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(filePath)
            };
        }

        public async Task UploadFromUrlAsync(string uri, string path, CancellationToken cancellationToken = default)
        {
            (bool isSuccess, var content) = await FileUtility.DownloadRemoteFileAsync(uri, cancellationToken);
            if (!isSuccess)
                throw new RestException(HttpStatusCode.InternalServerError, $"خطا در زمان دانلود {uri}");

            await _storage.WriteAsync(path, content, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string fullPath, CancellationToken cancellationToken)
        {
            await _storage.DeleteAsync(fullPath, cancellationToken);
        }

        public async Task DeleteRangeAsync(string[] fullPathList, CancellationToken cancellationToken)
        {
            await _storage.DeleteAsync(fullPathList, cancellationToken);
        }

        public async Task RenameAsync(string oldPath, string newPath, CancellationToken cancellationToken)
        {
            await _storage.RenameAsync(oldPath, newPath, cancellationToken);
        }

        public async Task<FileSize> GetTotallSize(string path, CancellationToken cancellationToken = default)
        {
            var files = await GetAllFiles(path, cancellationToken: cancellationToken);

            return new FileSize(files.Sum(x => x.Size) ?? 0);
        }
    }
}