using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels.SearchVM
{
    public class SearchVM
    {
        public List<Event> Events { get; set; }
        public List<GetSponsorDTO> Sponsors { get; set; } = new List<GetSponsorDTO>();
        public List<Speaker> Speakers { get; set; }
    }
}
