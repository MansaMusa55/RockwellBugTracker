using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockwellBugTracker.Services.Interfaces
{
    public interface IBTImageService
    {
        Task<byte[]> EncodeImageAsync(IFormFile file);
        Task<byte[]> EncodeImageAsync(string fileName);
        string DecodeImage(byte[] data, string type);
        bool ValidateImageType(IFormFile file);
        bool ValidateImageType(IFormFile file, List<string> fileTypes);
        bool ValidateImageSize(IFormFile file);
        bool ValidateImageSize(IFormFile file, int maxSize);
        string ContentType(IFormFile file);
        int Size(IFormFile file);
    }
}
