using EventEdu.Application.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFilesAsync(IFormFile file, string webRootPath, params string[] subfolders)
        {
            if (file == null || file.Length == 0)
                return null;

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            string folderPath = Path.Combine(webRootPath, Path.Combine(subfolders));
            string filePath = Path.Combine(folderPath, uniqueFileName);

            Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("/", Path.Combine(subfolders), uniqueFileName).Replace("\\", "/");
        }
    }
}
