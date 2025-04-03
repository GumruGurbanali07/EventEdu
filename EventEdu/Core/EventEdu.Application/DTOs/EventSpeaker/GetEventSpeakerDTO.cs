using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.EventSpeaker
{
    public class GetEventSpeakerDTO
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public Guid SpeakerId { get; set; }
        public string SpeakerName { get; set; }
    }
}
