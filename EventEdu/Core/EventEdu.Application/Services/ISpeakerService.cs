using EventEdu.Application.DTOs.Speaker;

namespace EventEdu.Application.Services
{
	public interface ISpeakerService
	{
		Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO);
		Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode);
		Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO);
		Task SoftDeleteSpeakerAsync(Guid speakerId);
		Task RestoreSpeakerAsync(Guid speakerId);
	}
}







