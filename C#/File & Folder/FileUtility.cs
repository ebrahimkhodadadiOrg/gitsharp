using FluentStorage.Model;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.FileUtility
{
    public static class FileUtility
    {
        static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string SizeSuffix(long value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
        }

        public static long MegabytesToBytes(double megabytes)
        {
            const int bytesInMegabyte = 1024 * 1024;
            return (long)(megabytes * bytesInMegabyte);
        }

        public static long BytesToMegabytes(long bytes)
        {
            return bytes / (1024 * 1024);
        }


        public static async Task<(bool isSuccess, long contentLength, string mimeType)>
            GetRemoteFileSizeAndMimeTypeAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                using (HttpClient client = new HttpClient(clientHandler))
                {
                    HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

                    var contentLength = response.Content.Headers.ContentLength ?? 0;
                    var mimeType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;

                    return (contentLength != -1 && !string.IsNullOrEmpty(mimeType), contentLength, mimeType);
                }
            }
            catch (Exception)
            {
                return (false, 0, "");
            }
        }

        public static async Task<(bool isSuccess, byte[] fileContent)>
            DownloadRemoteFileAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                using (HttpClient client = new HttpClient(clientHandler))
                {
                    HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                        var mimeType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;

                        return (true, fileBytes);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        public static bool GetFileInfoSizeAndMimeType(FileInfo file, out long ContentLength, out string MimeType)
        {
            try
            {
                ContentLength = file.Length;
                MimeType = Registry.GetValue(@$"HKEY_CLASSES_ROOT\{file.Extension}", "Content Type", null) as string;
                if (ContentLength == -1 || string.IsNullOrEmpty(MimeType))
                    return false;
                return true;
            }
            catch (Exception)
            {
                ContentLength = 0;
                MimeType = "";
                return false;
            }
        }
        public static bool GetBase64SizeAndMimeType(string base64, out long ContentLength, out string MimeType)
        {
            try
            {
                MimeType = "";
                ContentLength = Encoding.ASCII.GetByteCount(base64);

                var splited = base64.Split(';');
                if (splited.Length < 2 || !splited[0].Contains("data:"))
                    return false;

                splited = splited[0].Split(':');
                if (splited.Length < 2)
                    return false;

                MimeType = splited[1];
                if (ContentLength == -1 || string.IsNullOrEmpty(MimeType))
                    return false;
                return true;
            }
            catch (Exception)
            {
                ContentLength = 0;
                MimeType = "";
                return false;
            }

        }

        public static string GetFileExtentionBaseOnFile(UploadType uploadType)
        {
            return string.Join(",", MimeTypeManage.AccMimeTypeCollection[uploadType].Select(p => p.Extention));
        }
        public static bool CanAcceptFile(string MimeType, UploadType uploadType)
        {
            return MimeTypeManage.AccMimeTypeCollection[uploadType].Any(p => p.MimeType == MimeType);
        }
        //public static string CheckAndCreateAccessAddressDirectory(string AccessAddress)
        //{
        //    if (!Directory.Exists(AbsoluteHost + AccessAddress))
        //        Directory.CreateDirectory(AbsoluteHost + AccessAddress);

        //    return AbsoluteHost + AccessAddress;
        //}
        //public static string CheckAndCreateDirectory(string AccessAddress, string AbsoluteHost)
        //{
        //    var path = CheckAndCreateAccessAddressDirectory(AccessAddress, AbsoluteHost);

        //    if (!Directory.Exists(path + "/" + DateTime.Now.Year))
        //        Directory.CreateDirectory(path + "/" + DateTime.Now.Year);

        //    path += "/" + DateTime.Now.Year;
        //    if (!Directory.Exists(path + "/" + DateTime.Now.Month))
        //        Directory.CreateDirectory(path + "/" + DateTime.Now.Month);

        //    path += "/" + DateTime.Now.Month;
        //    return path;
        //}
        //public static string CheckAndCreateDirectoryStaticFile(string AccessAddress, string? staticFolder)
        //{
        //    var path = CheckAndCreateAccessAddressDirectory(AccessAddress);

        //    if (!Directory.Exists(path + "/" + staticFolder))
        //        Directory.CreateDirectory(path + "/" + staticFolder);

        //    path += "/" + staticFolder;
        //    return path;
        //}
        //public static string CheckDuplicateFileName(string fullPath)
        //{
        //    int count = 1;

        //    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
        //    string extension = Path.GetExtension(fullPath);
        //    string path = Path.GetDirectoryName(fullPath);
        //    string newFullPath = fullPath;

        //    while (File.Exists(newFullPath))
        //    {
        //        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
        //        newFullPath = Path.Combine(path, tempFileName + extension);
        //    }
        //    return newFullPath;
        //}
        public static string GetExtentionBaseOnMimeType(string mime)
        {
            string result;
            RegistryKey key;
            object value;

            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mime, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }
        //public static bool SaveImage64(byte[] Array, string FilePath)
        //{
        //    Image image;
        //    using (MemoryStream ms = new MemoryStream(Array))
        //        image = Image.FromStream(ms);
        //    image.Save(FilePath);
        //    return true;
        //}

        public static string ReadAllText(string file, bool errorHandler = false)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!fileInfo.Exists && errorHandler)
                throw new RestException(HttpStatusCode.NotFound, "فایل وجود ندارد");

            if (!fileInfo.Exists) return "";

            bool _isBinary = isBinary(file);
            if (_isBinary && errorHandler)
                throw new RestException(HttpStatusCode.BadRequest, "فرمت فایل صحیح نیست");

            if (_isBinary) return "";

            return File.ReadAllText(file);
        }

        public static bool isBinary(string path)
        {
            long length = new FileInfo(path).Length;
            if (length == 0) return false;

            using (StreamReader stream = new StreamReader(path))
            {
                int ch;
                while ((ch = stream.Read()) != -1)
                {
                    if (isControlChar(ch))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool isControlChar(int ch)
        {
            return ch > Chars.NUL && ch < Chars.BS
                || ch > Chars.CR && ch < Chars.SUB;
        }

        public static class Chars
        {
            public static char NUL = (char)0; // Null char
            public static char BS = (char)8; // Back Space
            public static char CR = (char)13; // Carriage Return
            public static char SUB = (char)26; // Substitute
        }
    }
}
