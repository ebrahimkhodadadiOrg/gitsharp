
using Domain.Models.Share;
using FluentStorage.Blobs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.FileUtility
{
    public interface IFileManager
    {
        /// <summary>
        /// Get all files with folders
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="includeSub">include sub files & folders</param>
        /// <param name="search">FullText search</param>
        /// <param name="count">Max count</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of files & folders</returns>
        Task<IReadOnlyCollection<Blob>> GetAll(string path, bool includeSub = true, string search = "", int? count = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all files
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="includeSub">include sub files</param>
        /// <param name="search">FullText search</param>
        /// <param name="count">Max count</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of files</returns>
        Task<IReadOnlyCollection<Blob>> GetAllFiles(string path, bool includeSub = true, string search = "", int? count = null,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="folderPath">path with folder name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>full path</returns>
        Task<string> CreateFolderAsync(string folderPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// download file from url and upload to provider
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="path">full path</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UploadFromUrlAsync(string uri, string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// upload image directly
        /// </summary>
        /// <param name="data">Image Array</param>
        /// <param name="path">full path</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UploadImageAsync(byte[] data, string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// upload file
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="path">full path</param>
        /// <param name="cancellationToken"></param>
        /// <returns>full path</returns>
        Task<string> UploadAsync(IFormFile file, string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Download from file provider
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <param name="cancellationToken"></param>
        /// <returns>byte array</returns>
        Task<byte[]> DownloadAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// remove file & folder
        /// </summary>
        /// <param name="fullPath">Full path</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(string fullPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// remove files & folders
        /// </summary>
        /// <param name="fullPathList">List of full path</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteRangeAsync(string[] fullPathList, CancellationToken cancellationToken = default);

        /// <summary>
        /// Rename file & Folder
        /// </summary>
        /// <param name="oldPath">Old Path</param>
        /// <param name="newPath">new Path</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RenameAsync(string oldPath, string newPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Totall Size of path
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>Full Size</returns>
        Task<FileSize> GetTotallSize(string path, CancellationToken cancellationToken = default);
    }
}
