using EventEdu.Domain.Entities;
using EventEdu.Domain.Entities.Common;
using EventEdu.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Context
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<CategoryDetail> CategoryDetails { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<CommentDetail> CommentDetails { get; set; }
		public DbSet<ContactInfo> ContactInfos { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventDetail> EventDetails { get; set; }
		public DbSet<EventSpeaker> EventSpeakers { get; set; }
		public DbSet<EventSponsor> EventSponsors { get; set; }
		public DbSet<FeedBack> FeedBacks { get; set; }
		public DbSet<FeedBackDetail> FeedBackDetails { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<Media> Medias { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<NotificationDetail> NotificationDetails { get; set; }
		public DbSet<Speaker> Speakers { get; set; }
		public DbSet<SpeakerDetail> SpeakerDetails { get; set; }
		public DbSet<Sponsor> Sponsors { get; set; }
		public DbSet<SponsorDetail> SponsorDetails { get; set; }
		public DbSet<UserEvent> UserEvents { get; set; }
		public DbSet<HeroSection> HeroSections { get; set; }
		public DbSet<HeroSectionDetail> HeroSectionDetails { get; set; }
		public DbSet<AboutSection> AboutSections { get; set; }
		public DbSet<AboutSectionDetail> AboutSectionDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// EventSpeaker əlaqəsi
			modelBuilder.Entity<EventSpeaker>()
				.HasKey(es => new { es.EventId, es.SpeakerId }); //composite key

			modelBuilder.Entity<EventSpeaker>()
				.HasOne(es => es.Event) 
				.WithMany(e => e.EventSpeakers) 
				.HasForeignKey(es => es.EventId); 

			modelBuilder.Entity<EventSpeaker>()
				.HasOne(es => es.Speaker) 
				.WithMany(s => s.EventSpeakers) 
				.HasForeignKey(es => es.SpeakerId); 

			// EventSponsor əlaqəsi
			modelBuilder.Entity<EventSponsor>()
				.HasKey(es => new { es.EventId, es.SponsorId }); 

			modelBuilder.Entity<EventSponsor>()
				.HasOne(es => es.Event) 
				.WithMany(e => e.EventSponsors) 
				.HasForeignKey(es => es.EventId); 

			modelBuilder.Entity<EventSponsor>()
				.HasOne(es => es.Sponsor) 
				.WithMany(s => s.EventSponsors) 
				.HasForeignKey(es => es.SponsorId); 

			// UserEvent əlaqəsi
			modelBuilder.Entity<UserEvent>()
				.HasKey(ue => new { ue.UserId, ue.EventId });

			modelBuilder.Entity<UserEvent>()
				.HasOne(ue => ue.User) 
				.WithMany(u => u.UserEvents) 
				.HasForeignKey(ue => ue.UserId); 

			modelBuilder.Entity<UserEvent>()
				.HasOne(ue => ue.Event) 
				.WithMany(e => e.UserEvents) 
				.HasForeignKey(ue => ue.EventId); 
		}

		public override async Task<int> SaveChangesAsync(CancellationToken token = default)
		{
			var entries = ChangeTracker.Entries();
			foreach (var entry in entries)
			{
				if (entry.Entity is BaseEntity entity)
				{
					switch (entry.State)
					{
						case EntityState.Added:
							entity.CreatedDate = DateTime.UtcNow;
							break;
						case EntityState.Modified:
							entity.UpdatedDate = DateTime.UtcNow;
							break;
					}
				}
			}
			return await base.SaveChangesAsync(token);
		}
	}
}
