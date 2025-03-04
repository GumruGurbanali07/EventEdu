using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Extensions
{
    public static class FileExtensions
    {
        public static async Task<string> SaveFilesAsync(this IFormFile file, string root, string client, string folderNameAssets, string folderNameImages, string folderName)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string path = Path.Combine(root, client, folderNameAssets, folderNameImages, folderName, uniqueFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using FileStream fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);
            return uniqueFileName;
        }

        public static bool CheckFileType(this IFormFile file, string fileType)
        {
            if (file.ContentType.StartsWith(fileType))
            {
                return true;
            }
            return false;
        }

        public static bool CheckFileSize(this IFormFile file, int fileSize)
        {
            if (file.Length < fileSize * 1024)
            {
                return true;
            }
            return false;
        }

        public static void DeleteFile(this IFormFile file, string root, string client, string folderNameAssets, string folderNameImages, string folderName, string fileName)
        {
            string path = Path.Combine(root, client, folderNameAssets, folderNameImages, folderName, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
