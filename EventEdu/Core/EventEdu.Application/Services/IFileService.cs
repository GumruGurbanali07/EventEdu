using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IFileService
    {
        Task<string> SaveFilesAsync(IFormFile file, string webRootPath, params string[] subfolders);
    }

}
