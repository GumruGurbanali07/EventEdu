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
using Microsoft.AspNetCore.Http;

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
		private readonly IEventSponsorReadRepository _eventSponsorReadRepository;
		private readonly IMapper _mapper;
		private readonly IValidator<CreateSponsorDTO> _createSponsorValidator;
		private readonly IHttpContextAccessor _httpContext;


		public SponsorService(AppDbContext context,
			ISponsorReadRepository sponsorReadRepository,
			ISponsorWriteRepository sponsorWriteRepository,
			ISponsorDetailReadRepository sponsorDetailReadRepository,
			ISponsorDetailWriteRepository sponsorDetailWriteRepository,
			ILanguageReadRepository languageReadRepository,
			IFileService fileService,
			IHostingEnvironment environment,
			IMapper mapper,
			IValidator<CreateSponsorDTO> createSponsorValidator,
			IHttpContextAccessor httpContext,
			IEventSponsorReadRepository eventSponsorReadRepository)
		{
			_context = context;
			_sponsorReadRepository = sponsorReadRepository;
			_sponsorWriteRepository = sponsorWriteRepository;
			_sponsorDetailReadRepository = sponsorDetailReadRepository;
			_sponsorDetailWriteRepository = sponsorDetailWriteRepository;
			_languageReadRepository = languageReadRepository;
			_fileService = fileService;

			_mapper = mapper;
			_createSponsorValidator = createSponsorValidator;
			_httpContext = httpContext;
			_eventSponsorReadRepository = eventSponsorReadRepository;
		}

		public async Task AddSponsor(CreateSponsorDTO addSponsorDTO)
		{
			//bool isSponsorExist = await _context.SponsorDetails.AnyAsync(x => x.SponsorName == addSponsorDTO.SponsorName && x.LanguageId == addSponsorDTO.LanguageId);

			//if (addSponsorDTO.ImageFile == null)
			//{
			//    throw new Exception("Image file is required.");
			//}

			var validationResult = await _createSponsorValidator.ValidateAsync(addSponsorDTO);



			var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == addSponsorDTO.LanguageId);




			if (!addSponsorDTO.ImageFile.CheckFileType("image"))
			{
				throw new Exception("Invalid file type. Please upload an image.");
			}

			if (!addSponsorDTO.ImageFile.CheckFileSize(10))
			{
				throw new Exception("File size is too large. Maximum allowed size is 10MB.");
			}


			string imagePath = await _fileService.UploadAsync(addSponsorDTO.ImageFile);


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

			await _sponsorWriteRepository.AddAsync(sponsor);
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

			await _sponsorDetailWriteRepository.AddAsync(sponsorDetail);
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
			var sponsors = await _sponsorReadRepository.GetByIdAsync(id.ToString());
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
			var sponsors = await _sponsorReadRepository.GetByIdAsync(id.ToString());
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
			var sponsor = await _sponsorReadRepository.GetAll()
				.Include(s => s.SponsorsDetail)
				.FirstOrDefaultAsync(s => s.Id == id);

			if (sponsor == null)
				throw new Exception("Sponsor not found.");

			var sponsorDetail = sponsor.SponsorsDetail
				.FirstOrDefault(sd => sd.SponsorId == id);

			if (sponsorDetail == null)
				throw new Exception("Sponsor detail for the selected language not found.");

		

			// Sponsor məlumatlarının yenilənməsi
			sponsor.Email = updateSponsorDTO.Email?.Trim() ?? sponsor.Email;
			sponsor.PhoneNumber = updateSponsorDTO.PhoneNumber?.Trim() ?? sponsor.PhoneNumber;
			sponsor.Website = updateSponsorDTO.Website?.Trim() ?? sponsor.Website;
			sponsor.UpdatedDate = DateTime.UtcNow.AddHours(4);

			// Şəkil dəyişdirilərsə
			if (updateSponsorDTO.ImageFile != null)
			{
				if (!updateSponsorDTO.ImageFile.CheckFileType("image"))
					throw new Exception("Invalid file type. Please upload an image.");

				if (!updateSponsorDTO.ImageFile.CheckFileSize(10))
					throw new Exception("File size is too large. Maximum allowed size is 10MB.");

				_fileService.Delete(sponsor.ImagePath);
				sponsor.ImagePath = await _fileService.UploadAsync(updateSponsorDTO.ImageFile);
			}
			// Verilənlərin saxlanması
			_sponsorWriteRepository.Update(sponsor);
			await _sponsorWriteRepository.SaveChangeAsync();
			var language = await _languageReadRepository.GetByIdAsync(updateSponsorDTO.LanguageId.ToString());
			// Sponsor detail məlumatlarının yenilənməsi
			sponsorDetail.LanguageId = language.Id;
			sponsorDetail.SponsorName = updateSponsorDTO.SponsorName?.Trim() ?? sponsorDetail.SponsorName;
			sponsorDetail.SponsorDescription = updateSponsorDTO.SponsorDescription?.Trim() ?? sponsorDetail.SponsorDescription;

	
			_sponsorDetailWriteRepository.Update(sponsorDetail);
			await _sponsorDetailWriteRepository.SaveChangeAsync();
			
		}


		public async Task<(List<Sponsor>, SponsorDetail)> GetSponsorAll()
		{

			var languages = _httpContext.HttpContext.Request.Headers["accept-language"].FirstOrDefault()?.Split(',').FirstOrDefault();
			var language = await _languageReadRepository.GetAll()
								.FirstOrDefaultAsync(a => a.IsoCode == languages);
			var sponsors = await _sponsorReadRepository.GetAll().ToListAsync();



			var sponsorDetails = await _sponsorDetailReadRepository.GetAll()
												.Where(a => a.LanguageId == language.Id).FirstOrDefaultAsync();



			// Bütün SponsorDetail-ləri birdəfəlik gətiririk ki, performans aşağı düşməsin



			return (sponsors, sponsorDetails);
		}

		public async Task<List<SponsorDetail>> GetSponsorsById(string eventId)
		{

			var languages = _httpContext?.HttpContext?.Request?.Headers["accept-language"].FirstOrDefault().Split(',').FirstOrDefault();

			var language = await _languageReadRepository.GetAll().FirstOrDefaultAsync(a => a.IsoCode == languages);
			var eventSponsor = await _eventSponsorReadRepository.GetAll().FirstOrDefaultAsync(a => a.EventId == Guid.Parse(eventId));

			var sponsor = await _sponsorDetailReadRepository.GetAll().Where(a => a.SponsorId == eventSponsor.SponsorId && a.LanguageId == language.Id).ToListAsync();

			return sponsor;
		}

        public async Task<List<GetSponsorDTO>> SearchSponsors(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return new List<GetSponsorDTO>();
            }

            var sponsors = await _sponsorReadRepository.GetAll()
                .Where(s => !s.IsDeleted &&
                            s.SponsorsDetail.Any(sd =>
                                sd.SponsorName.ToLower().Contains(search.ToLower()) ||
                                sd.SponsorDescription.ToLower().Contains(search.ToLower())))
                .Select(s => new GetSponsorDTO
                {
                    SponsorName = s.SponsorsDetail.FirstOrDefault().SponsorName,
                    ImagePath = s.ImagePath
                })
                .ToListAsync();
            return sponsors.GroupBy(s => s.SponsorName)
                           .Select(g => g.First())
                           .ToList();
        }


		public async Task<List<SponsorDetail>> GetSponsorDetails()
		{
			var languages = _httpContext?.HttpContext?.Request?.Headers["accept-language"].FirstOrDefault().Split(',').FirstOrDefault();

			var language = await _languageReadRepository.GetAll().FirstOrDefaultAsync(a => a.IsoCode == languages);
			var sponsorDetail = await _sponsorDetailReadRepository.GetAll().Where(a => a.LanguageId == language.Id).ToListAsync();
			return sponsorDetail;
		}
	}
}
