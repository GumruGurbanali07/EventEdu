using EventEdu.Application.Repositor;
using EventEdu.Application.Repository;
using EventEdu.Webui.ViewsModels.SearchVM;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{
    public class SearchController : Controller
    {
        private readonly IEventReadRepository _eventRepository;
        private readonly ISponsorReadRepository _sponsorRepository;
        private readonly ISpeakerReadRepository _speakerRepository;

        public SearchController(IEventReadRepository eventRepository, ISponsorReadRepository sponsorRepository, ISpeakerReadRepository speakerRepository)
        {
            _eventRepository = eventRepository;
            _sponsorRepository = sponsorRepository;
            _speakerRepository = speakerRepository;
        }

        public IActionResult Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new SearchVM());
            }

            var model = new SearchVM
            {
                //Events = _eventRepository.Search(query),
                Sponsors = _sponsorRepository.Search(query),
                //Speakers = _speakerRepository.Search(query)
            };

            return View(model);
        }
    }

}
