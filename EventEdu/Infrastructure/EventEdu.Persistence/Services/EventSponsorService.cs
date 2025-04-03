using EventEdu.Application.DTOs.EventSponsor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
    public class EventSponsorService : IEventSponsorService
    {
        private readonly IEventSponsorReadRepository _eventSponsorReadRepository;
        private readonly IEventSponsorWriteRepository _eventSponsorWriteRepository;

        public EventSponsorService(IEventSponsorReadRepository eventSponsorReadRepository, IEventSponsorWriteRepository eventSponsorWriteRepository)
        {
            _eventSponsorReadRepository = eventSponsorReadRepository;
            _eventSponsorWriteRepository = eventSponsorWriteRepository;
        }

        public async Task AddEventSponsorAsync(CreateEventSponsorDTO createEventSponsor)
        {
            var eventSponsor = new EventSponsor
            {
                EventId = createEventSponsor.EventId,
                SponsorId = createEventSponsor.SponsorId
            };

            await _eventSponsorWriteRepository.AddAsync(eventSponsor);
            await _eventSponsorWriteRepository.SaveChangeAsync();
        }

        public async Task RemoveEventSponsorAsync(Guid eventId, Guid sponsorId)
        {
            var eventSponsor = await _eventSponsorReadRepository.GetAll()
                .FirstOrDefaultAsync(es => es.EventId == eventId && es.SponsorId == sponsorId);

            if (eventSponsor != null)
            {
                _eventSponsorWriteRepository.Remove(eventSponsor);
                await _eventSponsorWriteRepository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<Sponsor>> GetSponsorsByEventIdAsync(Guid eventId)
        {
            return await _eventSponsorReadRepository.GetAll()
                .Where(es => es.EventId == eventId)
                .Select(es => es.Sponsor)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsBySponsorIdAsync(Guid sponsorId)
        {
            return await _eventSponsorReadRepository.GetAll()
                .Where(es => es.SponsorId == sponsorId)
                .Select(es => es.Event)
                .ToListAsync();
        }

        public async Task<bool> IsSponsorAssignedToEventAsync(Guid eventId, Guid sponsorId)
        {
            return await _eventSponsorReadRepository.GetAll()
                .AnyAsync(es => es.EventId == eventId && es.SponsorId == sponsorId);
        }
    }
}