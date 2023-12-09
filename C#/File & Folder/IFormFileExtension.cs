using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BuildingBlocks.FileUtility
{
    public static class FormFileExtension
    {
        public static byte[] ToByte(this IFormFile fli)
        {
            byte[] file = null;
            using (var fileStream = fli.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    file = ms.ToArray();
                }
            }
            return file;
        }
    }
}
