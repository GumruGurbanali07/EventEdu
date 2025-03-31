using EventEdu.Application.DTOs.Subscription;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface ISubscriptionService
    {
		Task SubscribeToEventAsync(SubscribeDTO subscribeDTO, Guid eventId);
		Task UnsubscribeFromEventAsync(string email, Guid eventId);

	}
}
