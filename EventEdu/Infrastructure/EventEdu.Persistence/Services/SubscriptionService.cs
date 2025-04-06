using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Application.DTOs.Subscription;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Services
{
	public class SubscriptionService : ISubscriptionService
	{
		private readonly ISubscriptionReadRepository _subscriptionReadRepository;
		private readonly IEventDetailReadRepository _eventDetailReadRepository;
		private readonly ISubscriptionWriteRepository _subscriptionWriteRepository;
		private readonly IEventReadRepository _eventReadRepository;
		private readonly ISubsEventWriteRepository _subsEventWriteRepository;
		private readonly ISubsEventReadRepository _subsEventReadRepository;
		private readonly IMailService _mailService;

		public SubscriptionService(
			ISubscriptionReadRepository subscriptionReadRepository,
			ISubscriptionWriteRepository subscriptionWriteRepository,
			IEventReadRepository eventReadRepository,
			ISubsEventWriteRepository subsEventWriteRepository,
			ISubsEventReadRepository subsEventReadRepository,
			IMailService mailService,
			IEventDetailReadRepository eventDetailReadRepository = null)
		{
			_subscriptionReadRepository = subscriptionReadRepository;
			_subscriptionWriteRepository = subscriptionWriteRepository;
			_eventReadRepository = eventReadRepository;
			_subsEventWriteRepository = subsEventWriteRepository;
			_subsEventReadRepository = subsEventReadRepository;
			_mailService = mailService;
			_eventDetailReadRepository = eventDetailReadRepository;
		}

		public async Task SubscribeToEventAsync(SubscribeDTO subscribeDTO, Guid eventId)
		{
			// Check if the event exists
			var eventEntity = await _eventReadRepository.GetByIdAsync(eventId.ToString());
			var eventDetails = await _eventDetailReadRepository.GetAll().FirstOrDefaultAsync(a => a.EventId == eventEntity.Id);
			if (eventEntity == null)
			{
				throw new ArgumentException("Event not found.");
			}

			// Check if the user is already subscribed to the event
			var existingSubscription = await _subscriptionReadRepository.GetAll()
				.Include(s => s.SubsEvents)
				.FirstOrDefaultAsync(s => s.Email == subscribeDTO.Email);

			if (existingSubscription != null && existingSubscription.SubsEvents.Any(se => se.EventId == eventId))
			{
				throw new InvalidOperationException("You are already subscribed to this event.");
			}

			// Create a new subscription if it doesn't exist
			if (existingSubscription == null)
			{
				existingSubscription = new Subscription
				{
					FirstName = subscribeDTO.FirstName,
					LastName = subscribeDTO.LastName,
					Email = subscribeDTO.Email,

				};
				await _subscriptionWriteRepository.AddAsync(existingSubscription);
				await _subscriptionWriteRepository.SaveChangeAsync();


			}

			// Link the subscription to the event
			var subsEvent = new SubsEvent
			{
				EventId = eventId,
				SubscriptionId = existingSubscription.Id
			};
			await _subsEventWriteRepository.AddAsync(subsEvent);

			// Save changes to the database
			await _subscriptionWriteRepository.SaveChangeAsync();
			string subject = "Event Registration Confirmation";

			string body = $@"
<html>
  <body style='font-family: Arial, sans-serif;'>
    <p>Dear {subscribeDTO.FirstName} {subscribeDTO.LastName},</p>

    <p>We are pleased to confirm your successful registration for the following event:</p>

    <p><strong>Event Name:</strong> {eventDetails.Title}<br/>
       <strong>Date:</strong> {eventEntity.GetFormattedStartDate()} – {eventEntity.GetFormattedEndDate()}</p>

   
    <p>We appreciate your interest and look forward to your participation. Should you have any questions or require further information, please do not hesitate to contact us.</p>

    <p>Best regards,<br/>
    Event Coordination Team</p>
  </body>
</html>";

			await _mailService.SendMailAsync(subscribeDTO.Email, subject, body);
		}

		public async Task UnsubscribeFromEventAsync(string email, Guid eventId)
		{
			// Check if the event exists
			var eventEntity = await _eventReadRepository.GetByIdAsync(eventId.ToString());
			if (eventEntity == null)
			{ 
				throw new ArgumentException("Event not found.");
			}

			// Find the subscription
			var subscription = await _subscriptionReadRepository.GetAll()
				.Include(s => s.SubsEvents)
				.FirstOrDefaultAsync(s => s.Email == email);

			if (subscription == null)
			{
				throw new ArgumentException("Subscription not found.");
			}

			// Find the subscription-event link
			var subsEvent = subscription.SubsEvents.FirstOrDefault(se => se.EventId == eventId);
			if (subsEvent == null)
			{
				throw new InvalidOperationException("You are not subscribed to this event.");
			}

			// Remove the subscription-event link
			_subsEventWriteRepository.Remove(subsEvent);

			// Save changes to the database
			await _subscriptionWriteRepository.SaveChangeAsync();

			// Send an unsubscription confirmation email
			string subject = "Unsubscription Confirmation";
			string body = $"Dear {subscription.FirstName} {subscription.LastName},\n\n" +
						  $"You have been unsubscribed from the event: {eventEntity.GetFormattedStartDate()} - {eventEntity.GetFormattedEndDate()}.\n\n" +
						  "We hope to see you again in the future!";

			await _mailService.SendMailAsync(email, subject, body);
		}
	}
}