using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Repository
{
    public interface ISpeakerWriteRepository : IWriteRepository<Speaker>
    {
		Task SoftDeleteSpeakerAsync(Guid speakerId);
		Task RestoreSpeakerAsync(Guid speakerId);
	}
}
