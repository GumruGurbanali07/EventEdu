using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using EventEdu.Persistence.Extensions;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Repository;
using EventEdu.Application.Repositor;
using FluentValidation;
using AutoMapper;

namespace EventEdu.Persistence.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly AppDbContext _context;
        private readonly ISponsorReadRepository _sponsorReadRepository;
        private readonly ISponsorWriteRepository _sponsorWriteRepository;
        private readonly ISponsorDetailReadRepository _sponsorDetailReadRepository;
        private readonly ISponsorDetailWriteRepository _sponsorDetailWriteRepository;
        private readonly ILanguageReadRepository _languageReadRepository;
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSponsorDTO> _createSponsorValidator;


        public SponsorService(AppDbContext context,
            ISponsorReadRepository sponsorReadRepository,
            ISponsorWriteRepository sponsorWriteRepository,
            ISponsorDetailReadRepository sponsorDetailReadRepository,
            ISponsorDetailWriteRepository sponsorDetailWriteRepository,
            ILanguageReadRepository languageReadRepository,
            IFileService fileService,
            IHostingEnvironment environment,
            IMapper mapper,
            IValidator<CreateSponsorDTO> createSponsorValidator)
        {
            _context = context;
            _sponsorReadRepository = sponsorReadRepository;
            _sponsorWriteRepository = sponsorWriteRepository;
            _sponsorDetailReadRepository = sponsorDetailReadRepository;
            _sponsorDetailWriteRepository = sponsorDetailWriteRepository;
            _languageReadRepository = languageReadRepository;
            _fileService = fileService;
            _environment = environment;
            _mapper = mapper;
            _createSponsorValidator = createSponsorValidator;
        }

        public async Task AddSponsor(CreateSponsorDTO addSponsorDTO)
        {
            //bool isSponsorExist = await _context.SponsorDetails.AnyAsync(x => x.SponsorName == addSponsorDTO.SponsorName && x.LanguageId == addSponsorDTO.LanguageId);

            //if (addSponsorDTO.ImageFile == null)
            //{
            //    throw new Exception("Image file is required.");
            //}

            var validationResult = await _createSponsorValidator.ValidateAsync(addSponsorDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }


            var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == addSponsorDTO.LanguageId);

            if (language == null)
            {
                throw new Exception("Selected language not found.");
            }


            if (!addSponsorDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addSponsorDTO.ImageFile.CheckFileSize(10))
            {
                throw new Exception("File size is too large. Maximum allowed size is 10MB.");
            }

            string webRootPath = _environment.WebRootPath;
            string imagePath = await _fileService.SaveFilesAsync(addSponsorDTO.ImageFile, webRootPath, "client", "assets", "img", "sponsorMedias");


            //var sponsor = new Sponsor
            //{
            //    Id = Guid.NewGuid(),
            //    Email = addSponsorDTO.Email.Trim(),
            //    PhoneNumber = addSponsorDTO.PhoneNumber.Trim(),
            //    Website = addSponsorDTO.Website.Trim(),
            //    ImagePath = imagePath.Trim(),
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow.AddHours(4),
            //    UpdatedDate = DateTime.UtcNow.AddHours(4)
            //};

            var sponsor = _mapper.Map<Sponsor>(addSponsorDTO);
            sponsor.Id = Guid.NewGuid();
            sponsor.ImagePath = imagePath;
            sponsor.IsDeleted = false;
            sponsor.CreatedDate = DateTime.UtcNow.AddHours(4);
            sponsor.UpdatedDate = DateTime.UtcNow.AddHours(4);

            ////////////var isSponsorExist = await _sponsorDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(sponsor.Id, addSponsorDTO.LanguageId);
            ////////////if (isSponsorExist != null)
            ////////////{
            ////////////    throw new Exception("This speaker already exists for the selected language.");
            ////////////}

            _sponsorWriteRepository.AddAsync(sponsor);
            await _sponsorWriteRepository.SaveChangeAsync();


            //var sponsorDetail = new SponsorDetail
            //{
            //    Id = Guid.NewGuid(),
            //    SponsorName = addSponsorDTO.SponsorName.Trim(),
            //    SponsorDescription = addSponsorDTO.SponsorDescription.Trim(),
            //    SponsorId = sponsor.Id,
            //    LanguageId = addSponsorDTO.LanguageId,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow.AddHours(4),
            //    UpdatedDate = DateTime.UtcNow.AddHours(4)
            //};

            var sponsorDetail = _mapper.Map<SponsorDetail>(addSponsorDTO);
            sponsorDetail.Id = Guid.NewGuid();
            sponsorDetail.SponsorId = sponsor.Id;
            sponsorDetail.CreatedDate = DateTime.UtcNow.AddHours(4);
            sponsorDetail.UpdatedDate = DateTime.UtcNow.AddHours(4);

            _sponsorDetailWriteRepository.AddAsync(sponsorDetail);
            await _sponsorDetailWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetSponsorDTO>> GetAllSponsorsByLanguageAsync(string isoCode)
        {

            var sponsors = await _sponsorReadRepository.GetAll()
                .Include(s => s.SponsorsDetail)
                .ThenInclude(sd => sd.Language)
                .Select(s => new GetSponsorDTO
                {
                    Id = s.Id,
                    IsoCode = s.SponsorsDetail.FirstOrDefault().Language.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Website = s.Website,
                    ImagePath = s.ImagePath,
                    IsDeleted = s.IsDeleted,
                    SponsorName = s.SponsorsDetail.FirstOrDefault().SponsorName,
                    SponsorDescription = s.SponsorsDetail.FirstOrDefault().SponsorDescription

                })
        .ToListAsync();


            return sponsors;
        }

        public async Task<GetSponsorDTO> GetSponsorById(Guid id, string isoCode)
        {
            var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);

            if (language == null)
            {
                throw new Exception("Invalid language selection.");
            }

            var sponsor = await _sponsorReadRepository.GetAll()
                  .Include(s => s.SponsorsDetail)
                  .ThenInclude(sd => sd.Language)
                  .Where(s => s.Id == id)
                  .Select(s => new GetSponsorDTO
                  {
                      Id = s.Id,
                      IsoCode = s.SponsorsDetail.FirstOrDefault().Language.Name,
                      LanguageId = s.SponsorsDetail.FirstOrDefault().Language.Id,
                      Email = s.Email,
                      PhoneNumber = s.PhoneNumber,
                      Website = s.Website,
                      ImagePath = s.ImagePath,
                      SponsorName = s.SponsorsDetail.FirstOrDefault().SponsorName,
                      SponsorDescription = s.SponsorsDetail.FirstOrDefault().SponsorDescription
                  })
                  .FirstOrDefaultAsync();

            return sponsor;
        }

        public async Task DeleteSponsor(Guid id)
        {
            var sponsors = await _sponsorReadRepository.GetByIdAsync(id);
            if (sponsors == null)
            {
                throw new Exception("Sponsor not found");
            }
            sponsors.SoftDelete();
            _sponsorWriteRepository.Update(sponsors);

            var sponsorDetails = await _sponsorDetailReadRepository.GetAll().Where(x => x.SponsorId == id).ToListAsync();
            foreach (var detail in sponsorDetails)
            {
                detail.SoftDelete();
                _sponsorDetailWriteRepository.Update(detail);
            }
            await _sponsorWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreSponsor(Guid id)
        {
            var sponsors = await _sponsorReadRepository.GetByIdAsync(id);
            if (sponsors == null)
            {
                throw new Exception("Sponsor not found");
            }
            sponsors.Restore();
            _sponsorWriteRepository.Update(sponsors);

            var sponsorDetails = await _sponsorDetailReadRepository.GetAll().Where(x => x.SponsorId == id).ToListAsync();
            foreach (var detail in sponsorDetails)
            {
                detail.Restore();
                _sponsorDetailWriteRepository.Update(detail);
            }
            await _sponsorWriteRepository.SaveChangeAsync();
        }

        public async Task EditSponsor(Guid id, CreateSponsorDTO updateSponsorDTO)
        {
            var sponsor = _sponsorReadRepository.GetAll()
            .Include(s => s.SponsorsDetail)
            .FirstOrDefault(s => s.Id == id
          );

            if (sponsor == null)
            {
                throw new Exception("Sponsor not found.");
            }
            var sponsorDetail = sponsor.SponsorsDetail
                .FirstOrDefault(sd => sd.SponsorId == id);

            if (sponsorDetail == null)
            {
                throw new Exception("Sponsor detail for the selected language not found.");
            }

            var validationResult = await _createSponsorValidator.ValidateAsync(updateSponsorDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            sponsor.Email = updateSponsorDTO.Email?.Trim() ?? sponsor.Email;
            sponsor.PhoneNumber = updateSponsorDTO.PhoneNumber?.Trim() ?? sponsor.PhoneNumber;
            sponsor.Website = updateSponsorDTO.Website?.Trim() ?? sponsor.Website;
            sponsor.ImagePath = updateSponsorDTO.ImagePath?.Trim() ?? sponsor.ImagePath;
            sponsor.UpdatedDate = DateTime.UtcNow.AddHours(4);

            sponsorDetail.LanguageId = updateSponsorDTO.LanguageId;
            sponsorDetail.SponsorName = updateSponsorDTO.SponsorName?.Trim() ?? sponsorDetail.SponsorName;
            sponsorDetail.SponsorDescription = updateSponsorDTO.SponsorDescription?.Trim() ?? sponsorDetail.SponsorDescription;
            sponsorDetail.UpdatedDate = DateTime.UtcNow.AddHours(4);

            if (updateSponsorDTO.ImageFile != null)
            {
                if (!updateSponsorDTO.ImageFile.CheckFileType("image"))
                {
                    throw new Exception("Invalid file type. Please upload an image.");
                }

                if (!updateSponsorDTO.ImageFile.CheckFileSize(10))
                {
                    throw new Exception("File size is too large. Maximum allowed size is 10MB.");
                }

                string webRootPath = _environment.WebRootPath;
                string newImagePath = await _fileService.SaveFilesAsync(updateSponsorDTO.ImageFile, webRootPath, "client", "assets", "img", "sponsorMedias");

                sponsor.ImagePath = newImagePath.Trim();
            }

            _sponsorWriteRepository.Update(sponsor);
            _sponsorDetailWriteRepository.Update(sponsorDetail);
            await _sponsorDetailWriteRepository.SaveChangeAsync();


        }

    }
}
