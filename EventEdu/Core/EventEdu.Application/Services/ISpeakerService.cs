using EventEdu.Application.DTOs;
using EventEdu.Application.DTOs.Speaker;
using EventEdu.Domain.Entities;

namespace EventEdu.Application.Services
{
	public interface ISpeakerService
	{
		Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO);

		Task<(List<Speaker> , SpeakerDetail)> GetSpeakersAllAsync();
		Task<List<SpeakerDetail>> GetSpeakersAsync();	
		Task<SpeakDetailsVM> GetSpeakersByIdAsync(string id );
		Task<List<SpeakerDetail>> GetSpeakersEventByIdAsync(string eventId);
		Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode);
		Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO);
		Task SoftDeleteSpeakerAsync(Guid speakerId);
		Task RestoreSpeakerAsync(Guid speakerId);
	}
}







