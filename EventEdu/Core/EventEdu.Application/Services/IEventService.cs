using EventEdu.Application.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IEventService
    {
        Task AddEventWithLanguageAsync(CreateEventDTO createEventDTO);
        Task<List<GetEventDTO>> GetEventsByLanguageAsync(string isoCode);
        Task<GetEventDTO?> GetEventByIdAndLanguageAsync(Guid eventId, string isoCode);
        Task UpdateEventAsync(Guid eventId, UpdateEventDTO updateEventDTO);
        Task SoftDeleteEventAsync(Guid eventId);
        Task RestoreEventAsync(Guid eventId);
    }
}