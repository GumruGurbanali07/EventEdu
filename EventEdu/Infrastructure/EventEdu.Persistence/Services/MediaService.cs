//using EventEdu.Application.DTOs.Media;

//using EventEdu.Application.Repository;
//using EventEdu.Application.Services;
//using EventEdu.Domain.Entities;
//using EventEdu.Persistence.Context;
//using EventEdu.Persistence.Extensions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EventEdu.Persistence.Services
//{
//    public class MediaService : IMediaService
//    {
//        private readonly AppDbContext _context;

//        public MediaService(AppDbContext context, string webRootPath)
//        {
//            _context = context;
    
//        }
//        public async Task AddMediaAsync(CreateMediaDTO createMediaDTO, string webRootPath)
//        {
//            if (createMediaDTO.File == null || createMediaDTO.File.Length == 0)
//            {
//                throw new ArgumentException("File cannot be null or empty.");
//            }

//            if (!createMediaDTO.File.CheckFileType("image"))
//            {
//                throw new ArgumentException("Invalid file type. Only images are allowed.");
//            }

//            if (!createMediaDTO.File.CheckFileSize(10000)) 
//            {
//                throw new ArgumentException("File size exceeds the 10MB limit.");
//            }

//            string uniqueFileName = await createMediaDTO.File.SaveFilesAsync(webRootPath, "client", "assets", "img", "medias");


//            var media = new Media
//            {
//                Id = Guid.NewGuid(),
//                FilePath = uniqueFileName,
//                UploadedDate = DateTime.UtcNow,
//                SponsorId = createMediaDTO.SponsorId,
//                HeroSectionId = createMediaDTO.HeroSectionId
//            };

//            _context.Medias.Add(media);
//            await _context.SaveChangesAsync();

//        }

//        public async Task<List<GetMediaDTO>> GetMediasAsync()
//        {
//            var medias = await _context.Medias
//                .Include(s => s.SponsorId)
//                .Include(s => s.HeroSectionId)
//            .Select(m => new GetMediaDTO
//            {
//                Id = m.Id,
//                FilePath = m.FilePath,
//                UploadedDate = m.UploadedDate,
//                SponsorId = m.SponsorId,
//                HeroSectionId = m.HeroSectionId
//            })
//            .ToListAsync();

//            return medias;
//        }

//        public async Task<GetMediaDTO> GetMediaById(Guid id)
//        {
//            var media = await _context.Medias.FindAsync(id);

//            if (media == null)
//            {
//                throw new KeyNotFoundException("Media not found.");
//            }

//            return new GetMediaDTO
//            {
//                Id = media.Id,
//                FilePath = media.FilePath,
//                UploadedDate = media.UploadedDate,
//                SponsorId = media.SponsorId,
//                HeroSectionId = media.HeroSectionId
//            };
//        }
//        public async Task<GetMediaDTO> DeleteMedia(Guid id, string webRootPath)
//        {
//            var media = await _context.Medias.FindAsync(id);

//            if (media == null)
//            {
//                throw new KeyNotFoundException("Media not found.");
//            }
//            string filePath = Path.Combine(webRootPath, "client", "assets", "img", "medias", media.FilePath);

//            if (System.IO.File.Exists(filePath))
//            {
//                System.IO.File.Delete(filePath);
//            }

//            _context.Medias.Remove(media);
//            await _context.SaveChangesAsync();

//            return new GetMediaDTO
//            {
//                Id = media.Id,
//                FilePath = media.FilePath,
//                UploadedDate = media.UploadedDate,
//                SponsorId = media.SponsorId,
//                HeroSectionId = media.HeroSectionId
//            };
//        }

//    }
//}
