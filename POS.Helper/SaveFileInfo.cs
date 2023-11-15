using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace POS.Helper
{
    public static class FileData
    {
        public static async Task SaveFile(string path, string source)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string base64 = source.Split(',').LastOrDefault();
            if (!string.IsNullOrWhiteSpace(base64))
            {
                byte[] bytes = Convert.FromBase64String(base64);
                await File.WriteAllBytesAsync(path, bytes);
            }
        }

        public static async Task SaveThumbnailFile(string path, string source)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var fileName = path.Split(Path.DirectorySeparatorChar).LastOrDefault();
            string base64 = source.Split(',').LastOrDefault();
            if (!string.IsNullOrWhiteSpace(base64))
            {
                byte[] bytes = Convert.FromBase64String(base64);
                var stream = new MemoryStream(bytes);
                IFormFile file = new FormFile(stream, 0, bytes.Length, fileName, fileName);

                using var image = await Image.LoadAsync(file.OpenReadStream());
                image.Mutate(x => x.Resize(100, 100));
                using var outputStream = new MemoryStream();
                image.Save(outputStream, new PngEncoder());
                await File.WriteAllBytesAsync(path, outputStream.ToArray());
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
