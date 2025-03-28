using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels.SearchVM
{
    public class SearchVM
    {
        public List<Event> Events { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public List<Speaker> Speakers { get; set; }
    }
}
