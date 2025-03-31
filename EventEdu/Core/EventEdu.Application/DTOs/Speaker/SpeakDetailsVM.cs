using EventEdu.Domain.Entities;
using E=EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs
{
  public  class SpeakDetailsVM
    {
        public E::Speaker Speaker { get; set; }
        public SpeakerDetail SpeakerDetail { get; set; }
    }
}
