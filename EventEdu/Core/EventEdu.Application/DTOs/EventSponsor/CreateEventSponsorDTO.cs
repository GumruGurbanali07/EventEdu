using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.EventSponsor
{
    public class CreateEventSponsorDTO
    {
        public Guid EventId { get; set; }
        public Guid SponsorId { get; set; }
    }
}
