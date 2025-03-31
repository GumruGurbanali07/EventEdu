using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Domain.Entities;

namespace EventEdu.Webui.Areas.Admin.ViewModels
{
    public class DashboardsVM
    {
        public List<Event> Events { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public List<Speaker> Speakers { get; set; }
    }
}
