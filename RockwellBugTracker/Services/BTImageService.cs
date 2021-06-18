using Microsoft.AspNetCore.Http;
using RockwellBugTracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Services
{
    public class BTImageService : IBTImageService
    {
        private const int DefaultMaxSize = (2 * 1024 * 1024);
        public string ContentType(IFormFile file)
        {
            return file?.ContentType.Split("/")[1];
        }

        public string DecodeImage(byte[] data, string type)
        {
            if (data is null || type is null) return null;
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile file)
        {
            if (file is null) return null;

            //This triggers more aggressive garbage collection
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();

        }

        public async Task<byte[]> EncodeImageAsync(string fileName)
        {
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{fileName}";
            return await File.ReadAllBytesAsync(file);
        }

        public int Size(IFormFile file)
        {
            return Convert.ToInt32(file?.Length);
        }

        public bool ValidateImageSize(IFormFile file)
        {
            return Size(file) < DefaultMaxSize; 
        }
        public bool ValidateImageSize(IFormFile file, int maxSize)
        {
            return Size(file) < maxSize; 
        }

        public bool ValidateImageType(IFormFile file)
        {
            var authorizedTypes = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
            var validExt = authorizedTypes.Contains(ContentType(file));
            return validExt;
        }

        public bool ValidateImageType(IFormFile file, List<string> fileTypes)
        {
            var validExt = fileTypes.Contains(ContentType(file));
            return validExt;
        }
    }
}
