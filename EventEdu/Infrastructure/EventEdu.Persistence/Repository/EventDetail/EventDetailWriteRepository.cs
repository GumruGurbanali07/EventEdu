using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Repository
{
    public class EventDetailWriteRepository : WriteRepository<EventDetail>, IEventDetailWriteRepository
    {
        public EventDetailWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
