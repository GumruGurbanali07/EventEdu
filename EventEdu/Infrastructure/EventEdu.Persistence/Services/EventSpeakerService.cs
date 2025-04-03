using EventEdu.Application.DTOs.EventSpeaker;
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
    public class EventSpeakerService : IEventSpeakerService
    {
        private readonly IEventSpeakerReadRepository _eventSpeakerReadRepository;
        private readonly IEventSpeakerWriteRepository _eventSpeakerWriteRepository;

        public EventSpeakerService(IEventSpeakerReadRepository eventSpeakerReadRepository, IEventSpeakerWriteRepository eventSpeakerWriteRepository)
        {
            _eventSpeakerReadRepository = eventSpeakerReadRepository;
            _eventSpeakerWriteRepository = eventSpeakerWriteRepository;
        }

        public async Task AddEventSpeakerAsync(CreateEventSpeakerDTO createEventSpeaker)
        {
            var eventSpeaker = new EventSpeaker
            {
                EventId = createEventSpeaker.EventId,
                SpeakerId = createEventSpeaker.SpeakerId
            };

            await _eventSpeakerWriteRepository.AddAsync(eventSpeaker);
            await _eventSpeakerWriteRepository.SaveChangeAsync();
        }

        public async Task RemoveEventSpeakerAsync(Guid eventId, Guid speakerId)
        {
            var eventSpeaker = await _eventSpeakerReadRepository.GetAll()
                .FirstOrDefaultAsync(es => es.EventId == eventId && es.SpeakerId == speakerId);

            if (eventSpeaker != null)
            {
                _eventSpeakerWriteRepository.Remove(eventSpeaker);
                await _eventSpeakerWriteRepository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersByEventIdAsync(Guid eventId)
        {
            return await _eventSpeakerReadRepository.GetAll()
                .Where(es => es.EventId == eventId)
                .Select(es => es.Speaker)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsBySpeakerIdAsync(Guid speakerId)
        {
            return await _eventSpeakerReadRepository.GetAll()
                .Where(es => es.SpeakerId == speakerId)
                .Select(es => es.Event)
                .ToListAsync();
        }

        public async Task<bool> IsSpeakerAssignedToEventAsync(Guid eventId, Guid speakerId)
        {
            return await _eventSpeakerReadRepository.GetAll()
                .AnyAsync(es => es.EventId == eventId && es.SpeakerId == speakerId);
        }
    }
}