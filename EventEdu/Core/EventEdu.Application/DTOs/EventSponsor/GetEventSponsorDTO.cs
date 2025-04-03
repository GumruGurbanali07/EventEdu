using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.EventSponsor
{
    public class GetEventSponsorDTO
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public Guid SponsorId { get; set; }
        public string SponsorName { get; set; }
    }
}
