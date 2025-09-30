using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // IWebHostEnvironment
using Microsoft.AspNetCore.Http;    // IFormFile
using AspMVC.Scripts.Classes.Enums;

namespace AspMVC.Services
{
    public class UploadMedia
    {
        private readonly IWebHostEnvironment _env;

        public UploadMedia(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<(string? url, PublicEnums.MediaType type)> SaveAsync(IFormFile? file)
        {
            if(file == null || file.Length == 0)
            {
                return (null, PublicEnums.MediaType.None);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var relativeUrl = $"/uploads/{uniqueName}";
            var mediaType = GetMediaType(file.ContentType,extension);
            return (relativeUrl,mediaType);
        }

        private PublicEnums.MediaType GetMediaType(string? contentType, string extension)
        {
            if(!string.IsNullOrEmpty(contentType))
            {
                if(contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    return PublicEnums.MediaType.Image;
                }
                if (contentType.StartsWith("Video/", StringComparison.OrdinalIgnoreCase))
                {
                    return PublicEnums.MediaType.Video;
                }
            }

            var imageExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
            var videoExt = new[] { ".mp4", ".webm", ".ogv", ".mov", ".avi" };

            if(imageExt.Contains(extension))
                return PublicEnums.MediaType.Image;
            if(videoExt.Contains(extension))
                return PublicEnums.MediaType.Video;

            return PublicEnums.MediaType.None;
        }
    }
}
