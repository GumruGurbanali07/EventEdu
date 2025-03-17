using EventEdu.Application.DTOs.EventSponsor;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IEventSponsorService
    {
        Task AddEventSponsorAsync(CreateEventSponsorDTO createEventSponsor);
        Task RemoveEventSponsorAsync(Guid eventId, Guid sponsorId);
        Task<IEnumerable<Sponsor>> GetSponsorsByEventIdAsync(Guid eventId);
        Task<IEnumerable<Event>> GetEventsBySponsorIdAsync(Guid sponsorId);
        Task<bool> IsSponsorAssignedToEventAsync(Guid eventId, Guid sponsorId);
    }

}
