using EventEdu.Application.DTOs.EventSpeaker;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IEventSpeakerService
    {
        Task AddEventSpeakerAsync(CreateEventSpeakerDTO createEventSpeaker);
        Task RemoveEventSpeakerAsync(Guid eventId, Guid speakerId);
        Task<IEnumerable<Speaker>> GetSpeakersByEventIdAsync(Guid eventId);
        Task<IEnumerable<Event>> GetEventsBySpeakerIdAsync(Guid speakerId);
        Task<bool> IsSpeakerAssignedToEventAsync(Guid eventId, Guid speakerId);
    }

}
