using EventEdu.Application.DTOs.Category;
using EventEdu.Application.DTOs.Speaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
	public interface ISpeakerService
	{
		Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO);
		Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode);
		Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO);
		Task SoftDeleteSpeakerAsnyc(Guid speakerId);
		Task RestoreSpeakerAsync(Guid speakerId);
	}
}
