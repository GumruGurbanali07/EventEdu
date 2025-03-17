using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.EventSpeaker
{
    public class CreateEventSpeakerDTO
    {
        public Guid EventId { get; set; }
        public Guid SpeakerId { get; set; }
    }
}
