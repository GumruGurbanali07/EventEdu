using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Event
{
    public class CreateEventDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FormattedStartDate => StartDate.ToString("yyyy-MM-dd HH:mm");
        public string FormattedEndDate => EndDate.ToString("yyyy-MM-dd HH:mm");
        public Guid LanguageId { get; set; }
    }
}